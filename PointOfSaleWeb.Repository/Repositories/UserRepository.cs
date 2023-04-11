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
                var userData = await db.QuerySingleOrDefaultAsync<UserInfoDTO>("GetUserData", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<DbResponse<UserInfoDTO>> CreateUser(User userData)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Username", userData.Username);
            parameters.Add("@Password", userData.Password);
            parameters.Add("@FirstName", userData.FirstName);
            parameters.Add("@LastName", userData.LastName);
            parameters.Add("@Email", userData.Email);

            try
            {
                var user = await db.QuerySingleOrDefaultAsync<UserInfoDTO>("CreateUser", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<UserInfoDTO>
                {
                    Success = true,
                    Data = user
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

        public async Task<DbResponse<UserInfoDTO>> UpdateUser(User user)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@UserID", user.UserID);
            parameters.Add("@Username", user.Username);
            parameters.Add("@Password", user.Password);
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@UserRoleID", user.UserRoleID);

            try
            {
                var userModified = await db.QuerySingleOrDefaultAsync<UserInfoDTO>("UpdateUser", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<UserInfoDTO>
                {
                    Success = true,
                    Message = "User updated!",
                    Data = userModified
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

        public async Task<DbResponse<UserInfoDTO>> DeleteUser(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@UserID", id);

            try
            {
                await db.ExecuteAsync("DeleteUser", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<UserInfoDTO>
                {
                    Success = true
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
