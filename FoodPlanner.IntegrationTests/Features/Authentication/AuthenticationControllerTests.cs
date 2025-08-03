using System.Net.Http.Json;
using FluentAssertions;
using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Features.Login;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Core.Constants;
using FoodPlanner.IntegrationTests.Helpers;
using Xunit;

namespace FoodPlanner.IntegrationTests.Features.Authentication;

public class AuthenticationControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthenticationControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ReturnsSuccessAndToken_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginUserCommand
        {
            Username = "Testuser",
            Password = "Password123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Console.WriteLine(response);
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
        apiResponse.Should().NotBeNull();
        apiResponse!.Success.Should().BeTrue();
        apiResponse.Data.Should().NotBeNull();
        apiResponse.Data!.TokenInfo.Token.Should().NotBeNullOrEmpty();
        apiResponse.Message.Should().Be(AuthenticationMessages.LoginSuccessfull); 

        // Можно проверить, что expiry дата валидна
        apiResponse.Data.TokenInfo.Expires.Should().BeAfter(DateTime.UtcNow);
    }

}