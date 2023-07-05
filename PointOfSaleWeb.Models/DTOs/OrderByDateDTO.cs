namespace PointOfSaleWeb.Models.DTOs
{
    public class OrderByDateDTO
    {
        public Guid OrderID { get; set; }
        public string User { get; set; } = string.Empty;  
        public decimal OrderTotal { get; set; }
    }
}
