using FoodPlanner.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FoodPlanner.Tests.Common;

public static class IdentityMockHelper
{
    public static Mock<UserManager<TUser>> CreateUserManagerMock<TUser>() where TUser : class
    {
        var userManagerMock = new Mock<UserManager<TUser>>(
            new Mock<IUserStore<TUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<TUser>>().Object,
            new IUserValidator<TUser>[0],
            new IPasswordValidator<TUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<TUser>>>().Object);
        
        return userManagerMock;
    }

    public static Mock<RoleManager<TRole>> CreateRoleManagerMock<TRole>() where TRole : class
    {
        var roleManagerMock = new Mock<RoleManager<TRole>>(
            new Mock<IRoleStore<TRole>>().Object,
            new IRoleValidator<TRole>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<TRole>>>().Object);
        
         return roleManagerMock;
    }
}