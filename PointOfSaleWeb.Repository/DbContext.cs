using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace PointOfSaleWeb.Repository
{
    public class DbContext
    {
        private readonly IConfiguration? _configuration;
        private readonly string? _connectionString;

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnMonsterAsp");
        }

        public IDbConnection CreateConnection()
        {
            IDbConnection conn;

            try
            {
                conn = new MySqlConnection(_connectionString);
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