namespace PointOfSaleWeb.Models.DTOs
{
    public class RecentOrderDTO
    {
        public string User { get; set; } = string.Empty;
        public Guid OrderID { get; set; } 
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
