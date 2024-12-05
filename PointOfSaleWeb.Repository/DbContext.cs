using Microsoft.Data.SqlClient;
using System.Data;

namespace PointOfSaleWeb.Repository
{
    public class DbContext
    {
        private readonly string? _connectionString;

        public DbContext()
        {
            _connectionString = Environment.GetEnvironmentVariable("DB_CONN");

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Database connection string is missing. Please set the 'DB_CONN' environment variable.");
            }
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