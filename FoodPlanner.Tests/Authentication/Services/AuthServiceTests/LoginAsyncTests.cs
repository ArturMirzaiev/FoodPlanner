using System.Security.Claims;
using FluentAssertions;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Responses;
using FoodPlanner.Infrastructure.Services;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Authentication.Services.AuthServiceTests;

public class LoginAsyncTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IUserClaimsService> _claimsServiceMock; 
    
    public LoginAsyncTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _claimsServiceMock = new Mock<IUserClaimsService>();
    }
    
    [Fact]
    public async Task LoginAsync_Should_ReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "user", Password = "pass" };
        var user = new ApplicationUser();
        var claims = new List<Claim> { new Claim("sub", "userId") };
        var token = new LoginResponseDto { Token = "jwt-token", Expires = DateTime.UtcNow.AddMinutes(60) };

        _userServiceMock.Setup(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userServiceMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _claimsServiceMock.Setup(x => x.GetClaimsAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(claims);
        _jwtTokenServiceMock.Setup(x => x.GenerateToken(claims))
            .ReturnsAsync(token);

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _claimsServiceMock.Object
        );

        // Act
        var result = await authService.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<LoginResponseDto>>();
        result.IsSuccess.Should().BeTrue();
        result.Value?.Token.Should().Be(token.Token);
        result.Value?.Expires.Should().Be(token.Expires);
        result.Errors.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public async Task LoginAsync_Should_ReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "invalid" };
        var expectedErrors = new[] { "Invalid username or password" };
        
        _userServiceMock.Setup(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplicationUser?)null);

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _claimsServiceMock.Object
        );

        // Act
        var result = await authService.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<LoginResponseDto>>();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(expectedErrors);
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_Should_ReturnFailure_WhenPasswordIsInvalid()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "user", Password = "pass" };
        var user = new ApplicationUser();
        var expectedErrors = new[] { "Invalid username or password" };
        
        _userServiceMock.Setup(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userServiceMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _claimsServiceMock.Object
        );
        
        // Act
        var result = await authService.LoginAsync(loginDto, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<Result<LoginResponseDto>>();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(expectedErrors);
        result.Value.Should().BeNull();
        
        _userServiceMock.Verify(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(x => x.CheckPasswordAsync(user, loginDto.Password, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_Should_ReturnFailure_WhenClaimsAreNull()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "user", Password = "pass" };
        var user = new ApplicationUser { UserName = "test" };
        List<Claim>? claims = null;
        var expectedErrors = new[] { "User has no claims" };
        
        _userServiceMock.Setup(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);;
        _userServiceMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);;
        _claimsServiceMock
            .Setup(x => x.GetClaimsAsync(user, It.IsAny<CancellationToken>()))!
            .ReturnsAsync(claims);
        
        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _claimsServiceMock.Object
        );
        
        // Act
        var result = await authService.LoginAsync(loginDto, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<Result<LoginResponseDto>>();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(expectedErrors);
        result.Value.Should().BeNull();
        
        _userServiceMock.Verify(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(x => x.CheckPasswordAsync(user, loginDto.Password, It.IsAny<CancellationToken>()), Times.Once);
        _claimsServiceMock.Verify(x => x.GetClaimsAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task LoginAsync_Should_ReturnFailure_WhenClaimsAreEmpty()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "user", Password = "pass" };
        var user = new ApplicationUser { UserName = "test" };
        var expectedErrors = new[] { "User has no claims" };

        _userServiceMock.Setup(x => x.FindByNameAsync(loginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userServiceMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _claimsServiceMock.Setup(x => x.GetClaimsAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Claim>());

        var authService = new AuthService(
            _userServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _claimsServiceMock.Object
        );

        // Act
        var result = await authService.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<LoginResponseDto>>();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(expectedErrors);
        result.Value.Should().BeNull();
    }
}