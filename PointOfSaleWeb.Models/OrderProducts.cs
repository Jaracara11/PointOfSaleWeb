using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class OrderProducts
    {
        [Required]
        public string ProductID { get; set; } = string.Empty;

        [Required]
        public int ProductQuantity { get; set; }
    }
}
