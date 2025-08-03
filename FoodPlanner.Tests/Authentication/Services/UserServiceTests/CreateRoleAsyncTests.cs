using FluentAssertions;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Infrastructure.Services;
using FoodPlanner.Tests.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Authentication.Services.UserServiceTests;

public class CreateRoleAsyncTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;

    public CreateRoleAsyncTests()
    {
        _userManagerMock = IdentityMockHelper.CreateUserManagerMock<ApplicationUser>();
        _roleManagerMock = IdentityMockHelper.CreateRoleManagerMock<IdentityRole<Guid>>();
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldThrow_WhenIdentityResultFailed()
    {        
        // Arrange
        var roleName = "invalidRole";
        var cts = CancellationToken.None;
        var identityErrors = new IdentityError[]
        {
            new IdentityError
            {
                Code = "ShortName",
                Description = "Short name"
            },
            new IdentityError
            {
                Code = "InvalidSymbols",
                Description = "Invalid symbols"
            }
        };

        _roleManagerMock.Setup(r => r.CreateAsync(
        It.Is<IdentityRole<Guid>>(role => role.Name == roleName)))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));
        
        
        var userService = new UserService(_userManagerMock.Object, _roleManagerMock.Object);

        // Act
        var act = () => userService.CreateRoleAsync(roleName, cts);

        // Assert
        var ex = await act.Should().ThrowAsync<InvalidOperationException>();
        ex.Which.Message.Should().Contain(identityErrors[0].Code);
        ex.Which.Message.Should().Contain(identityErrors[1].Code);
        ex.Which.Message.Should().Contain(identityErrors[0].Description);
        ex.Which.Message.Should().Contain(identityErrors[1].Description);
        
        _roleManagerMock.Verify(r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()), Times.Once);
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldBeSuccessful_WhenIdentityResultSucceeded()
    {
        // Arrange
        var roleName = "invalidRole";
        var cts = CancellationToken.None;
        
        _roleManagerMock.Setup(r => r.CreateAsync(
                It.Is<IdentityRole<Guid>>(role => role.Name == roleName)))
            .ReturnsAsync(IdentityResult.Success);
        
        var userService = new UserService(_userManagerMock.Object, _roleManagerMock.Object);
        
        // Act
        var act = () => userService.CreateRoleAsync(roleName, cts);
        
        // Assert
        await act.Should().NotThrowAsync();
        
        _roleManagerMock.Verify(r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()), Times.Once);
    }
}