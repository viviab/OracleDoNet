using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Habitania.OracleDbConnection.Interfaces;
using Oracle.DataAccess.Client;

namespace Habitania.OracleDbConnection
{
    public class OracleDbCommand : IOracleDbCommand
    {
        private readonly IDbConnectionFactory _dbConnection;
        public OracleDbCommand(IDbConnectionFactory dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<T> GetList<T>(String query, IEnumerable<OracleParameter> parameters = null, params Object[] values) where T : class
        {
            using (var dbConnection = _dbConnection.CreateConnection())
            {
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Clear();
                    
                    if (parameters != null) parameters.ToList().ForEach(item => cmd.Parameters.Add(item));

                    var oReader = cmd.ExecuteReader();
                    var lReturn = oReader.Select(reader => (T)Activator.CreateInstance(typeof(T), new Object[] { reader, values }));
                    return lReturn.ToList();
                }
            }
        }

        public List<int> GetListInt(string query, IEnumerable<OracleParameter> parameters = null)
        {
            using (var dbConnection = _dbConnection.CreateConnection())
            {
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Clear();
                    if (parameters != null) parameters.ToList().ForEach(item => cmd.Parameters.Add(item));

                    var oReader = cmd.ExecuteReader();
                    List<int> oReturn = new List<int>();
                    while (oReader.Read())
                        oReturn.Add(oReader.GetInt32(0));

                    return oReturn;
                }
            }
        }


        public int Execute(String query, IEnumerable<OracleParameter> parameters = null)
        {

            using (var dbConnection = _dbConnection.CreateConnection())
            {
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Clear();
                    if (parameters != null) parameters.ToList().ForEach(item => cmd.Parameters.Add(item));

                    var result = cmd.ExecuteNonQuery();
                    return result;
                }
            }

        }

        public int ExecuteInsert(String query, IEnumerable<OracleParameter> parameters = null)
        {

            using (var dbConnection = _dbConnection.CreateConnection())
            {
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Clear();
                    if (parameters != null) parameters.ToList().ForEach(item => cmd.Parameters.Add(item));

                    cmd.Parameters.Add(new OracleParameter(":ID", OracleDbType.Decimal, ParameterDirection.Output));
                    cmd.ExecuteNonQuery();

                    var result = int.Parse(cmd.Parameters[":ID"].ToString());
                    return result;

                }
            }
        }

        public int ExecuteScalar(string query, IEnumerable<OracleParameter> parameters = null)
        {
            using (var dbConnection = _dbConnection.CreateConnection())
            {
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Clear();
                    parameters.ToList().ForEach(item => cmd.Parameters.Add(item));

                    var result = cmd.ExecuteScalar();

                    if (result==null || result == DBNull.Value)
                        return 0;

                    return int.Parse(result.ToString());
                }
            }
        }

        public string ExecuteScalarString(String query, IEnumerable<OracleParameter> parameters = null)
        {
            using (var dbConnection = _dbConnection.CreateConnection())
            {
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Clear();
                    if (parameters != null) parameters.ToList().ForEach(item => cmd.Parameters.Add(item));

                    var result = cmd.ExecuteScalar();
                    if ( result == null || result == DBNull.Value)
                        return string.Empty;

                        return result.ToString();
                }
            }
        }

        public bool ExecuteNonQuery(String query, IEnumerable<OracleParameter> parameters = null)
        {
            using (var dbConnection = _dbConnection.CreateConnection())
            {
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Clear();
                    if (parameters != null) parameters.ToList().ForEach(item => cmd.Parameters.Add(item));

                    var result = cmd.ExecuteNonQuery();
                    return (result != 0);
                }
            }
        }

        public T Find<T>(String query, List<OracleParameter> parameters = null, CultureInfo cultureGlobal = null) where T : class
        {
            try
            {
                Object[] values = new Object[] { cultureGlobal };
                var result = GetList<T>(query, parameters, values);
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
