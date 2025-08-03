using FluentAssertions;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Infrastructure.Services;
using FoodPlanner.Tests.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Authentication.Services.UserServiceTests;

public class CreateUserAsyncTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;

    public CreateUserAsyncTests()
    {
        _userManagerMock = IdentityMockHelper.CreateUserManagerMock<ApplicationUser>();
        _roleManagerMock = IdentityMockHelper.CreateRoleManagerMock<IdentityRole<Guid>>();
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrow_WhenIdentityResultFailed()
    {
        // Arrange
        var user = new ApplicationUser();
        var password = "password";
        var cts = new CancellationTokenSource().Token;
        var identityError = new IdentityError
        {
            Code = "FailedCreation",
            Description = "Failed Creation"
        };
        
        _userManagerMock.Setup(u => u.CreateAsync(user, password))
            .ReturnsAsync(IdentityResult.Failed(identityError));
        
        var userService = new UserService(_userManagerMock.Object, _roleManagerMock.Object);
        // Act
        var act = () => userService.CreateUserAsync(user, password, cts);
        //Assert
        var ex = await act.Should().ThrowAsync<InvalidOperationException>();
        ex.Which.Message.Should().Contain(identityError.Code);
        ex.Which.Message.Should().Contain(identityError.Description);
        
        _userManagerMock.Verify(u => u.CreateAsync(It.IsAny<ApplicationUser>(), password), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldBeSuccessful_WhenIdentityResultSucceeded()
    {
        // Arrange
        var user = new ApplicationUser();
        var password = "password";
        var cts = new CancellationTokenSource().Token;
        
        _userManagerMock.Setup(u => u.CreateAsync(user, password))
            .ReturnsAsync(IdentityResult.Success);
        
        var userService = new UserService(_userManagerMock.Object, _roleManagerMock.Object);
        
        // Act
        var act = () => userService.CreateUserAsync(user, password, cts);
        
        //Assert
        await act.Should().NotThrowAsync();
        
        _userManagerMock.Verify(u => u.CreateAsync(It.IsAny<ApplicationUser>(), password), Times.Once);
    }
}