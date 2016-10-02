using System.Configuration;
using System.Data;
using Habitania.OracleDbConnection.Interfaces;

namespace Habitania.OracleDbConnection
{
    public class OracleConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public OracleConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public OracleConnectionFactory()
            : this(ConfigurationManager.AppSettings["OracleConnectionString"])
        {

        }

        public IDbConnection CreateConnection()
        {
            var conn = new Oracle.DataAccess.Client.OracleConnection(_connectionString);
            conn.Open();
            return conn;
        }

    }
}
