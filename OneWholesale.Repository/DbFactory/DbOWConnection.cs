using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OneWholesale.Repository.DbFactory
{
    public class DbOWConnection : IDbOWConnection
    {
        private readonly string _connectionString;

        public DbOWConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentException("DefaultConnection string is missing in configuration");

            Connection = new SqlConnection(_connectionString);
        }

        public SqlConnection Connection { get; private set; }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
