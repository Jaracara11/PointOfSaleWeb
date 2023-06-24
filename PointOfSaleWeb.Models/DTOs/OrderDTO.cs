namespace PointOfSaleWeb.Models.DTOs
{
    public class OrderDTO
    {
        public string OrderID { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public Product[] Products { get; set; } = Array.Empty<Product>();
        public decimal OrderSubTotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal OrderTotal { get; set; }
        public string OrderDate { get; set; } = string.Empty;
    }
}
