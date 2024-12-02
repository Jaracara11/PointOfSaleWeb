namespace PointOfSaleWeb.Models.DTOs
{
    public class UserAuthResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public int? UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}