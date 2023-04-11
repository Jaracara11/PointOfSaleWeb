using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models.DTOs
{
    public class UserChangePasswordDTO
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Username must have between 3 and 50 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Password must have between 4 and 50 characters.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Old password cannot contain white spaces.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Password must have between 4 and 50 characters.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "New password cannot contain white spaces.")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
