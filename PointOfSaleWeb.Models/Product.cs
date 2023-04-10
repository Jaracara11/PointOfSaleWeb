using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Product name must have between 3 and 50 characters.")]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Product description cannot exceed 100 characters.")]
        public string ProductDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product quantity is required.")]
        public int ProductStock { get; set; }

        [Required(ErrorMessage = "Product cost is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product cost must be greater than zero.")]
        public decimal ProductCost { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than zero.")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Product category is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Product category ID must be greater than zero.")]
        public int ProductCategoryID { get; set; }
    }
}
