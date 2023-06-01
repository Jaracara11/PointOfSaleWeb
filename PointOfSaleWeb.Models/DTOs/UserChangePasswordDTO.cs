namespace PointOfSaleWeb.Models.DTOs
{
    public class UserChangePasswordDTO
    {
        public string Username { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
