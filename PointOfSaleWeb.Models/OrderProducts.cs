using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class OrderProducts
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int ProductQuantity { get; set; }
    }
}
