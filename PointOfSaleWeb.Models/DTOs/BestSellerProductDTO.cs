namespace PointOfSaleWeb.Models.DTOs
{
    public class BestSellerProductDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
    }
}
