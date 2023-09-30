namespace PointOfSaleWeb.Models.DTOs
{
    public class ProductSoldByDateDTO
    {
        public string ProductID { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal TotalSold { get; set; }
    }
}
