using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class OrderRequest
    {
        [Required]
        public string User { get; set; } = string.Empty;

        [Required]
        public OrderProducts[] Products { get; set; } = Array.Empty<OrderProducts>();

        [Range(0.01, double.MaxValue, ErrorMessage = "Discount must be greater than zero.")]
        public decimal? Discount { get; set; }
    }
}
