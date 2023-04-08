using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Category name must be between 4 and 50 characters.")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
