using Dapper;
using Dapper.Contrib.Extensions;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository.Repositories
{
    public class UserRepository(DbContext context) : IUserRepository
    {
        private readonly DbContext _context = context;

        public async Task<IEnumerable<UserDataDTO>> GetAllUsers()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.GetAllAsync<UserDataDTO>();
        }

        public async Task<UserDataDTO?> GetUserByUsername(string username)
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.QuerySingleOrDefaultAsync<UserDataDTO>(
                "SELECT UserID, Username, Email, FirstName, LastName, UserRoleID FROM Users WHERE Username = @Username",
                new { Username = username }
            );
        }

        public async Task<IEnumerable<UserRole>> GetAllUserRoles()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.GetAllAsync<UserRole>();
        }

        public async Task<UserAuthResponseDTO?> AuthUser(UserAuthDTO user)
        {
            using IDbConnection db = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Password", user.Password);

            return await db.QuerySingleOrDefaultAsync<UserAuthResponseDTO>(
                "sp_AuthUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<UserInfoDTO?> AddNewUser(User userData)
        {
            using IDbConnection db = _context.CreateConnection();

            var result = await db.QueryAsync<UserInfoDTO>(
                "CreateUser",
                MapUserToParameters(userData),
                commandType: CommandType.StoredProcedure
            );

            return result.FirstOrDefault();
        }

        public async Task<UserInfoDTO?> UpdateUser(UserDataDTO user)
        {
            using IDbConnection db = _context.CreateConnection();

            var result = await db.QueryAsync<UserInfoDTO>(
                "UpdateUser",
                MapUserToParameters(user),
                commandType: CommandType.StoredProcedure
            );

            return result.FirstOrDefault();
        }

        public async Task<bool> ChangeUserPassword(UserChangePasswordDTO userData)
        {
            using IDbConnection db = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Username", userData.Username);
            parameters.Add("@OldPassword", userData.OldPassword);
            parameters.Add("@NewPassword", userData.NewPassword);

            var rowsAffected = await db.ExecuteAsync("ChangeUserPassword", parameters, commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }

        public async Task<bool> ResetUserPassword(UserChangePasswordDTO userData)
        {
            using IDbConnection db = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Username", userData.Username);
            parameters.Add("@NewPassword", userData.NewPassword);

            var rowsAffected = await db.ExecuteAsync("ResetUserPassword", parameters, commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUser(string username)
        {
            using IDbConnection db = _context.CreateConnection();

            var user = new { Username = username };
            var rowsAffected = await db.ExecuteAsync("DeleteUser", user, commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }

        private static DynamicParameters MapUserToParameters(User user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Password", user.Password);
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@UserRoleID", user.UserRoleID);

            return parameters;
        }

        private static DynamicParameters MapUserToParameters(UserDataDTO user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@UserRoleID", user.UserRoleID);

            return parameters;
        }
    }
}