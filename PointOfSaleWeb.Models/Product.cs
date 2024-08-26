using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class Product
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Product ID must have between 3 and 50 characters.")]
        public string ProductID { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Product name must have between 3 and 50 characters.")]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Product description cannot exceed 100 characters.")]
        public string ProductDescription { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Product stock cannot be negative.")]
        public int ProductStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Product quantity cannot be negative.")]
        public int ProductQuantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product cost must be greater than zero.")]
        public decimal ProductCost { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than zero.")]
        public decimal ProductPrice { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Category name must have between 4 and 50 characters.")]
        public string ProductCategoryName { get; set; } = string.Empty;
    }
}
