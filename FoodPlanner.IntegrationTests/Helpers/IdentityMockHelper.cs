using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FoodPlanner.IntegrationTests.Helpers;

public static class IdentityMockHelper
{
    public static Mock<UserManager<TUser>> CreateUserManagerMock<TUser>() where TUser : class
    {
        var userStoreMock = new Mock<IUserStore<TUser>>();
        return new Mock<UserManager<TUser>>(
            userStoreMock.Object,
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<TUser>>(),
            Array.Empty<IUserValidator<TUser>>(),
            Array.Empty<IPasswordValidator<TUser>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<TUser>>>());
    }

    public static Mock<RoleManager<TRole>> CreateRoleManagerMock<TRole>() where TRole : class
    {
        var roleStoreMock = new Mock<IRoleStore<TRole>>();
        return new Mock<RoleManager<TRole>>(
            roleStoreMock.Object,
            Array.Empty<IRoleValidator<TRole>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<ILogger<RoleManager<TRole>>>());
    }
}
