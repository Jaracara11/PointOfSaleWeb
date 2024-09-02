using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.Tests.Utilities
{
    public class TestDbResponse<T> : DbResponse<T>
    {
        public TestDbResponse(bool success, T? data, string message = "")
        {
            Success = success;
            Data = data;
            Message = message;
        }
    }
}
