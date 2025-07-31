using FluentAssertions;
using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Services.Contracts;
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

    public RegisterAsyncTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
    }

    [Fact]
    public async Task RegisterAsync_WhenValidInput_ReturnsCorrectRegisterResponseDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var registerDto = new RegisterDto
        {
            Username = "Username12345!",
            Password = "password",
            Email = "email@gmail.com",
            Role = UserRole.User
        };
        var expectedRole = registerDto.Role.ToString();
        ApplicationUser? createdUser = null;

        _userServiceMock.Setup(x =>
                x.CreateUserAsync(It.IsAny<ApplicationUser>(), registerDto.Password, It.IsAny<CancellationToken>()))
            .Callback<ApplicationUser, string, CancellationToken>((user, _, _) =>
            {
                user.Id = userId;
                createdUser = user;
            })
            .Returns(Task.CompletedTask);

        _userServiceMock.Setup(x => x.RoleExistsAsync(expectedRole, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userServiceMock.Setup(x => x.CreateRoleAsync(expectedRole, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _userServiceMock.Setup(x =>
            x.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), expectedRole, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var authService = new AuthService(_userServiceMock.Object, _jwtTokenServiceMock.Object);
        
        // Act
        var result = await authService.RegisterAsync(registerDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be(registerDto.Username);
        result.Role.Should().Be(expectedRole);
        result.UserId.Should().Be(userId);

        createdUser.Should().NotBeNull();
        createdUser!.UserName.Should().Be(registerDto.Username);
        createdUser.Email.Should().Be(registerDto.Email);

        _userServiceMock.Verify(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), registerDto.Password, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(x => x.RoleExistsAsync(expectedRole, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(x => x.CreateRoleAsync(expectedRole, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(x => x.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), expectedRole, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task RegisterAsync_WhenDtoIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        RegisterDto dto = null!;
        var authService = new AuthService(_userServiceMock.Object, _jwtTokenServiceMock.Object);

        // Act
        Func<Task> act = async () => await authService.RegisterAsync(dto, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("dto");
    }
}
