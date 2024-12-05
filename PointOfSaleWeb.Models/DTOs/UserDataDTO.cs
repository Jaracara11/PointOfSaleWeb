namespace PointOfSaleWeb.Models.DTOs
{
    [Dapper.Contrib.Extensions.Table("Users")]
    public class UserDataDTO
    {
        [Dapper.Contrib.Extensions.Key]
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public int UserRoleID { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}