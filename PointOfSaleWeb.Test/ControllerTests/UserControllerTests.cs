using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PointOfSaleWeb.App.Controllers;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;
        private readonly UserController _controller;
        private readonly List<UserDataDTO> _users;
        private readonly List<Role> _roles;

        public UserControllerTests()
        {
            _userRepo = A.Fake<IUserRepository>();
            _configuration = A.Fake<IConfiguration>();
            _controller = new UserController(_userRepo, _configuration);

            _users =
            [
                new()
            {
                Username = "user1",
                FirstName = "First",
                LastName = "User",
                Email = "user1@example.com",
                UserRoleID = 1
            },
            new()
            {
                Username = "user2",
                FirstName = "Second",
                LastName = "User",
                Email = "user2@example.com",
                UserRoleID = 2
            },
            new()
            {
                Username = "user3",
                FirstName = "Third",
                LastName = "User",
                Email = "user3@example.com",
                UserRoleID = 3
            }
            ];

            _roles =
            [
                new () { RoleID = 1, RoleName = "Admin" },
                new () { RoleID = 2, RoleName = "User" },
                new () { RoleID = 3, RoleName = "Manager" }
            ];
        }

        [Fact]
        public async Task GetAllUsers_Returns_Ok()
        {
            // Arrange
            A.CallTo(() => _userRepo.GetAllUsers()).Returns(Task.FromResult((IEnumerable<UserDataDTO>)_users));

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedUsers = okResult.Value.Should().BeOfType<List<UserDataDTO>>().Subject;
            returnedUsers.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetUserByUsername_ValidUsername_Returns_Ok()
        {
            // Arrange
            var username = "user1";
            var expectedUser = _users.Find(u => u.Username == username);
            A.CallTo(() => _userRepo.GetUserByUsername(username)).Returns(Task.FromResult(expectedUser));

            // Act
            var result = await _controller.GetUserByUsername(username);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedUser = okResult.Value.Should().BeOfType<UserDataDTO>().Subject;
            returnedUser.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetUserByUsername_InvalidUsername_Returns_NotFound()
        {
            // Arrange
            var username = "nonexistentUser";
            A.CallTo(() => _userRepo.GetUserByUsername(username)).Returns(Task.FromResult<UserDataDTO?>(null));

            // Act
            var result = await _controller.GetUserByUsername(username);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAllUserRoles_Returns_Ok()
        {
            // Arrange
            A.CallTo(() => _userRepo.GetAllUserRoles())
                .Returns(Task.FromResult<IEnumerable<Role>>(_roles));

            // Act
            var result = await _controller.GetAllUserRoles();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedRoles = okResult.Value.Should().BeOfType<List<Role>>().Subject;
            returnedRoles.Should().HaveCount(3);
        }

        [Fact]
        public async Task CreateUser_Success_Returns_Created()
        {
            // Arrange
            var newUser = new User
            {
                Username = "newuser",
                FirstName = "New",
                LastName = "User",
                Email = "newuser@example.com",
                Password = "password"
            };

            var userDataDto = new UserInfoDTO
            {
                Username = "newuser",
                Name = "New User",
                Email = "newuser@example.com",
                Role = "Admin",
                Token = "fake-jwt-token"
            };

            var response = new DbResponse<UserInfoDTO>
            {
                Success = true,
                Data = userDataDto
            };

            A.CallTo(() => _userRepo.CreateUser(newUser)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.CreateUser(newUser);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedResult>().Subject;
            createdResult.Location.Should().Be("User");
            createdResult.Value.Should().BeEquivalentTo(userDataDto);
        }

        [Fact]
        public async Task UpdateUser_Success_Returns_Ok()
        {
            // Arrange
            var userToUpdate = new UserDataDTO
            {
                Username = "existinguser",
                FirstName = "Updated",
                LastName = "User",
                Email = "updateduser@example.com",
                UserRoleID = 2
            };

            var response = new DbResponse<UserInfoDTO>
            {
                Success = true,
                Data = new UserInfoDTO
                {
                    Username = "existinguser",
                    Name = "Updated User",
                    Email = "updateduser@example.com",
                    Role = "User",
                    Token = "fake-jwt-token"
                }
            };

            A.CallTo(() => _userRepo.UpdateUser(userToUpdate)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.UpdateUser(userToUpdate);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task ChangeUserPassword_Success_Returns_NoContent()
        {
            // Arrange
            var userChangePasswordDto = new UserChangePasswordDTO
            {
                Username = "user1",
                OldPassword = "oldPassword123",
                NewPassword = "newPassword123"
            };

            var response = new DbResponse<string>
            {
                Success = true
            };

            A.CallTo(() => _userRepo.ChangeUserPassword(userChangePasswordDto)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.ChangeUserPassword(userChangePasswordDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ResetUserPassword_Success_Returns_NoContent()
        {
            // Arrange
            var userChangePasswordDto = new UserChangePasswordDTO
            {
                Username = "user1",
                OldPassword = "oldPassword123",
                NewPassword = "newPassword123"
            };

            var response = new DbResponse<string>
            {
                Success = true
            };

            A.CallTo(() => _userRepo.ResetUserPassword(userChangePasswordDto)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.ResetUserPassword(userChangePasswordDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteUser_Success_Returns_NoContent()
        {
            // Arrange
            var username = "userToDelete";
            var response = new DbResponse<UserInfoDTO>
            {
                Success = true
            };

            A.CallTo(() => _userRepo.DeleteUser(username)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.DeleteUser(username);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }
    }
}