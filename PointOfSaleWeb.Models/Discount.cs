using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class Discount
    {
        [Key]
        public int UserRoleID { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
