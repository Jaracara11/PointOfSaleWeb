using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace PointOfSaleWeb.Repository
{
    public class SecurityDbContext
    {
        private readonly IConfiguration? _configuration;
        private readonly string? _connectionString;

        public SecurityDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SecurityDbConn");
        }

        public IDbConnection CreateConnection()
        {
            IDbConnection conn;

            try
            {
                conn = new SqlConnection(_connectionString);
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
                throw;
            }

            return conn;
        }
    }
}
