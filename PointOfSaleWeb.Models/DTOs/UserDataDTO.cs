namespace PointOfSaleWeb.Models.DTOs
{
    public class UserDataDTO
    {
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int UserRoleID { get; set; }
    }
}
