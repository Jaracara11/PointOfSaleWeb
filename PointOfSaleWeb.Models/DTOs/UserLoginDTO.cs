using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        [StringLength(25)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Password must have between 4 and 50 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
