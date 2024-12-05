using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDataDTO>> GetAllUsers();
        Task<UserDataDTO?> GetUserByUsername(string username);
        Task<IEnumerable<UserRole>> GetAllUserRoles();
        Task<UserAuthResponseDTO?> AuthUser(UserAuthDTO user);
        Task<UserInfoDTO?> AddNewUser(User userData);
        Task<UserInfoDTO?> UpdateUser(UserDataDTO user);
        Task<bool> ChangeUserPassword(UserChangePasswordDTO userData);
        Task<bool> ResetUserPassword(UserChangePasswordDTO userData);
        Task<bool> DeleteUser(string username);
    }
}
