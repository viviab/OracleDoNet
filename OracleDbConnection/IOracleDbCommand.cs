using System;
using System.Collections.Generic;
using System.Globalization;
using Oracle.DataAccess.Client;
using System.Globalization;

namespace Habitania.OracleDbConnection
{
    public interface IOracleDbCommand
    {
        List<T> GetList<T>(String query, IEnumerable<OracleParameter> parameters = null, params Object[] values) where T : class;
        List<int> GetListInt(string query, IEnumerable<OracleParameter> parameters = null);
        int Execute(String query, IEnumerable<OracleParameter> parameters = null);
        int ExecuteInsert(String query, IEnumerable<OracleParameter> parameters = null);
        int ExecuteScalar(string query, IEnumerable<OracleParameter> parameters);
        string ExecuteScalarString(String query, IEnumerable<OracleParameter> parameters = null);
        bool ExecuteNonQuery(String query, IEnumerable<OracleParameter> parameters = null);
        T Find<T>(String query, List<OracleParameter> parameters = null, CultureInfo cultureGlobal = null) where T:class;
    }
}