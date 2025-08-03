using FluentAssertions;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Infrastructure.Services;
using FoodPlanner.Tests.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Authentication.Services.UserServiceTests;

public class AddUserToRoleAsyncTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;

    public AddUserToRoleAsyncTests()
    {
        _userManagerMock = IdentityMockHelper.CreateUserManagerMock<ApplicationUser>();
        _roleManagerMock = IdentityMockHelper.CreateRoleManagerMock<IdentityRole<Guid>>();
    }

    [Fact]
    public async Task AddUserToRoleAsync_ShouldThrow_WhenIdentityResultIsFailed()
    {
        // Arrange
        var user = new ApplicationUser();
        var roleName = "invalidRole";
        var identityError = new IdentityError
        {
            Code = "ErrorCode", 
            Description = "TestRole is invalid"
        };

        _userManagerMock
            .Setup(u => u.AddToRoleAsync(user, roleName))
            .ReturnsAsync(IdentityResult.Failed(identityError));
        
        var userService = new UserService(_userManagerMock.Object, _roleManagerMock.Object);
        
        // Act
        Func<Task> act = () => userService.AddUserToRoleAsync(user, roleName, CancellationToken.None);
        
        // Assert
        var ex = await act.Should().ThrowAsync<InvalidOperationException>();
        ex.Which.Message.Should().Contain(identityError.Code);
        ex.Which.Message.Should().Contain(identityError.Description);
        
        _userManagerMock.Verify(u => u.AddToRoleAsync(user, roleName), Times.Once);
    }

    [Fact]
    public async Task AddUserToRoleAsync_ShouldBeSuccessful_WhenIdentityResultSucceeded()
    {
        // Arrange
        var user = new ApplicationUser();
        var roleName = "validRole";
        
        _userManagerMock
            .Setup(u => u.AddToRoleAsync(user, roleName))
            .ReturnsAsync(IdentityResult.Success);
        
        var userService = new UserService(_userManagerMock.Object, _roleManagerMock.Object);
        
        // Act
        var act = () => userService.AddUserToRoleAsync(user, roleName, CancellationToken.None);
        
        // Asert
        await act.Should().NotThrowAsync();
        
        _userManagerMock.Verify(u => u.AddToRoleAsync(user, roleName), Times.Once);
    }
}