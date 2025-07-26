using FluentAssertions;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Enums;
using FoodPlanner.Infrastructure.Services;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Authentication.Services.AuthServiceTests;

public class RegisterAsyncTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IUserClaimsService> _userClaimsServiceMock;

    public RegisterAsyncTests()
    {
        _userClaimsServiceMock = new Mock<IUserClaimsService>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _userServiceMock = new Mock<IUserService>();
    }

    [Fact]
    public async Task RegisterAsync_ReturnsSuccessResult_WithCorrectData()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "Password123!",
            Role = UserRole.User
        };

        var createdUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = dto.Username,
            Email = dto.Email
        };

        ApplicationUser? createdUserInCallback = null;

        _userServiceMock
            .Setup(s => s.CreateUserAsync(It.Is<ApplicationUser>(u => u.UserName == dto.Username && u.Email == dto.Email), dto.Password, It.IsAny<CancellationToken>()))
            .Callback<ApplicationUser, string, CancellationToken>((user, password, ct) =>
            {
                user.Id = createdUser.Id;
                createdUserInCallback = user;
            })
            .ReturnsAsync(Result.Success());

        _userServiceMock
            .Setup(s => s.RoleExistsAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userServiceMock
            .Setup(s => s.CreateRoleAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _userServiceMock
            .Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success())
            .Callback<ApplicationUser, string, CancellationToken>((user, role, ct) =>
            {
                user.Should().BeSameAs(createdUserInCallback);
            });

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _userClaimsServiceMock.Object);

        // Act
        var result = await authService.RegisterAsync(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<RegisterResponseDto>>()
            .Which.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.UserId.Should().Be(createdUser.Id);
        result.Value.Username.Should().Be(dto.Username);
        result.Value.Role.Should().Be(dto.Role.ToString());

        _userServiceMock.Verify(s => s.CreateUserAsync(It.Is<ApplicationUser>(u => u.UserName == dto.Username && u.Email == dto.Email), dto.Password, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.RoleExistsAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.CreateRoleAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.AddUserToRoleAsync(It.Is<ApplicationUser>(u => u == createdUserInCallback), dto.Role.ToString(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task RegisterAsync_ReturnsFailure_WhenCreateUserFails()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "Password123!",
            Role = UserRole.User
        };
        var errors = new[] { "Failed to create user" };

        _userServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<ApplicationUser>(), dto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(errors));

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _userClaimsServiceMock.Object);

        // Act
        var result = await authService.RegisterAsync(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<RegisterResponseDto>>();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
        result.Value.Should().BeNull();

        _userServiceMock.Verify(s => s.CreateUserAsync(It.IsAny<ApplicationUser>(), dto.Password, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.RoleExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _userServiceMock.Verify(s => s.CreateRoleAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _userServiceMock.Verify(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsFailure_WhenCreateRoleFails()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "Password123!",
            Role = UserRole.User
        };

        var createdUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = dto.Username,
            Email = dto.Email
        };

        var errors = new[] { "Failed to create role" };

        _userServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<ApplicationUser>(), dto.Password, It.IsAny<CancellationToken>()))
            .Callback<ApplicationUser, string, CancellationToken>((user, password, ct) => user.Id = createdUser.Id)
            .ReturnsAsync(Result.Success());

        _userServiceMock
            .Setup(s => s.RoleExistsAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userServiceMock
            .Setup(s => s.CreateRoleAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(errors));

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _userClaimsServiceMock.Object);

        // Act
        var result = await authService.RegisterAsync(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<RegisterResponseDto>>();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
        result.Value.Should().BeNull();

        _userServiceMock.Verify(s => s.CreateUserAsync(It.IsAny<ApplicationUser>(), dto.Password, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.RoleExistsAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.CreateRoleAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsFailure_WhenAddUserToRoleFails()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "Password123!",
            Role = UserRole.User
        };

        var errors = new[] { "Failed to add role to user" };

        _userServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<ApplicationUser>(), dto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        _userServiceMock
            .Setup(s => s.RoleExistsAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _userServiceMock
            .Setup(s => s.CreateRoleAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        _userServiceMock
            .Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(errors));

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _userClaimsServiceMock.Object);

        // Act
        var result = await authService.RegisterAsync(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<RegisterResponseDto>>();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
        result.Value.Should().BeNull();

        _userServiceMock.Verify(s => s.CreateUserAsync(It.IsAny<ApplicationUser>(), dto.Password, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.RoleExistsAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.CreateRoleAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), dto.Role.ToString(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task RegisterAsync_DoesNotCreateRole_IfRoleAlreadyExists()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "Password123!",
            Role = UserRole.User
        };

        var createdUser = new ApplicationUser { Id = Guid.NewGuid(), UserName = dto.Username, Email = dto.Email };

        _userServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<ApplicationUser>(), dto.Password, It.IsAny<CancellationToken>()))
            .Callback<ApplicationUser, string, CancellationToken>((user, password, ct) => user.Id = createdUser.Id)
            .ReturnsAsync(Result.Success());

        _userServiceMock
            .Setup(s => s.RoleExistsAsync(dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);  // роль уже существует

        _userServiceMock
            .Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), dto.Role.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _userClaimsServiceMock.Object);

        // Act
        var result = await authService.RegisterAsync(dto, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        _userServiceMock.Verify(s => s.CreateRoleAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
