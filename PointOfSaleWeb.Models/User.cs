using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Username must have between 3 and 50 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Password must have between 4 and 50 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "FirstName must have between 3 and 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "LastName must have between 3 and 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "User role ID must be greater than zero.")]
        public int UserRoleID { get; set; }
    }
}
