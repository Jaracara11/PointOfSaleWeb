using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<DbResponse<UserInfoDTO>> GetUserData(UserLoginDTO user);
    }
}
