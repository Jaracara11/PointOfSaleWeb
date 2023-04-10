using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(25)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Password must have between 4 and 50 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int UserRoleID { get; set; }
    }
}
