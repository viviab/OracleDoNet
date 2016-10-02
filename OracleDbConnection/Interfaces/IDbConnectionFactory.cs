using System.Data;

namespace Habitania.OracleDbConnection.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
