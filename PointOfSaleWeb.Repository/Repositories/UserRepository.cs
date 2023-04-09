using Dapper;
using Microsoft.Data.SqlClient;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SecurityDbContext _context;
        public UserRepository(SecurityDbContext context)
        {
            _context = context;
        }

        public async Task<DbResponse<UserInfoDTO>> GetUserData(UserLoginDTO user)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Password", user.Password);

            try
            {
                var userData = await db.QuerySingleOrDefaultAsync<UserInfoDTO>("GetUserLoginData", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<UserInfoDTO>
                {
                    Success = true,
                    Data = userData
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse<UserInfoDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
