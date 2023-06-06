using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserInfoDTO>> GetAllUsersInfo();
        Task<UserDataDTO> GetUserByUsername(string username);
        Task<IEnumerable<Role>> GetAllUserRoles();
        Task<DbResponse<UserInfoDTO>> AuthUser(UserLoginDTO user);
        Task<DbResponse<UserInfoDTO>> CreateUser(User user);
        Task<DbResponse<UserInfoDTO>> UpdateUser(UserDataDTO user);
        Task<DbResponse<string>> ChangeUserPassword(UserChangePasswordDTO userData);
        Task<DbResponse<string>> ResetUserPassword(UserChangePasswordDTO userData);
        Task<DbResponse<UserInfoDTO>> DeleteUser(string username);
    }
}
