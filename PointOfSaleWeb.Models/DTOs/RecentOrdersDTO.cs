namespace PointOfSaleWeb.Models.DTOs
{
    public class RecentOrdersDTO
    {
        public string User { get; set; } = string.Empty;
        public string Products { get; set; } = string.Empty;
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
