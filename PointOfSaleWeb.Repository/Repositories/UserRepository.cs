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
        private readonly DbContext _context;
        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserInfoDTO>> GetAllUsersInfo()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.QueryAsync<UserInfoDTO>("GetAllUsersInfo", commandType: CommandType.StoredProcedure);
        }

        public async Task<User> GetUserByID(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", id);

            return await db.QuerySingleOrDefaultAsync<User>("GetUserById", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Role>> GetAllUserRoles()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.QueryAsync<Role>("GetAllUserRoles", commandType: CommandType.StoredProcedure);
        }

        public async Task<DbResponse<UserInfoDTO>> AuthUser(UserLoginDTO user)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Password", user.Password);

            try
            {
                var userData = await db.QuerySingleOrDefaultAsync<UserInfoDTO>("AuthUser", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<DbResponse<UserInfoDTO>> UpdateUser(UserDataDTO user)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
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

        public async Task<DbResponse<string>> ChangeUserPassword(UserChangePasswordDTO user)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@OldPassword", user.OldPassword);
            parameters.Add("@NewPassword", user.NewPassword);

            try
            {
                await db.ExecuteAsync("ChangeUserPassword", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<string>
                {
                    Success = true,
                    Message = "Password changed successfully!"
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DbResponse<UserInfoDTO>> DeleteUser(string username)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Username", username);

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
