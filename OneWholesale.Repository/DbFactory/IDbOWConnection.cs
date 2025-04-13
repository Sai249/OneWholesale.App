using System.Data;
using Microsoft.Data.SqlClient;

namespace OneWholesale.Repository.DbFactory
{
    public interface IDbOWConnection
    {
        IDbConnection GetConnection();
        IDbConnection CreateConnection();
        SqlConnection Connection { get; }
    }
}