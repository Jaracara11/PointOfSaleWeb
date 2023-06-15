namespace PointOfSaleWeb.Models
{
    public class Discount
    {
        public int UserRoleID { get; set; }
        public string[] DiscountsAvailable { get; set; } = Array.Empty<string>();
    }
}
