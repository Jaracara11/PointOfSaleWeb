using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 50 characters.")]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Product description cannot exceed 100 characters.")]
        public string ProductDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product quantity is required.")]
        public int ProductStock  { get; set; }

        [Required(ErrorMessage = "Product cost is required.")]
        public decimal ProductCost  { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Product category is required.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Product category must be between 4 and 50 characters.")]
        public int ProductCategoryID { get; set; }
    }
}
