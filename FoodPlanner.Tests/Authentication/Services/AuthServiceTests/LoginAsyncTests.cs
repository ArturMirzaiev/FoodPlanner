using FluentAssertions;
using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Services.Contracts;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Enums;
using FoodPlanner.Infrastructure.Services;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Authentication.Services.AuthServiceTests;

public class LoginAsyncTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    
    public LoginAsyncTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
    }
    
    [Fact]
    public async Task LoginAsync_ShouldReturnLoginResponse_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "user", Password = "pass" };
        var user = new ApplicationUser();
        var roles = new List<string> { nameof(UserRole.User) };
        var token = new JwtTokenDto
        {
            Token = "jwt-token", 
            Expires = DateTime.UtcNow.AddMinutes(60)
        };

        _userServiceMock.Setup(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userServiceMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _userServiceMock.Setup(x => x.GetUserRolesAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(roles);
        _jwtTokenServiceMock.Setup(x => x.GenerateTokenAsync(user, It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(token);

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object
        );

        // Act
        var result = await authService.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<LoginResponseDto>();
        result.Username.Should().Be(loginDto.Username);
        result.TokenInfo.Token.Should().Be(token.Token);
        result.TokenInfo.Expires.Should().Be(token.Expires);
    }
    
    [Fact]
    public async Task LoginAsync_ShouldThrowAuthenticationException_WhenUserNotFound()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "invalid" };
        var expectedErrorMessage = $"User with username '{loginDto.Username}' was not found.";
        
        _userServiceMock.Setup(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplicationUser?)null);

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object
        );

        // Act
        Func<Task> act = () => authService.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<KeyNotFoundException>();
        ex.WithMessage(expectedErrorMessage);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowAuthenticationException_WhenPasswordIsInvalid()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "user", Password = "pass" };
        var user = new ApplicationUser();
        var expectedErrorMessage = "Invalid password.";
        
        _userServiceMock.Setup(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userServiceMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object
        );
        
        // Act
        Func<Task> act = () => authService.LoginAsync(loginDto, CancellationToken.None);
        
        // Assert
        var ex = await act.Should().ThrowAsync<UnauthorizedAccessException>();
        ex.WithMessage(expectedErrorMessage);
    }
}