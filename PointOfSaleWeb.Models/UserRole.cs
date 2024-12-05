using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class UserRole
    {
        [Dapper.Contrib.Extensions.Key]
        public int UserRoleID { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
