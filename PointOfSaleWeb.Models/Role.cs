using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
