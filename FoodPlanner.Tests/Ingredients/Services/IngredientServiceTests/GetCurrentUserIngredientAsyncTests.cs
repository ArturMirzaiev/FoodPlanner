using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Shared.Services;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Interfaces;
using FoodPlanner.Infrastructure.Services;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Ingredients.Services.IngredientServiceTests;

public class GetCurrentUserIngredientAsyncTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    
    public GetCurrentUserIngredientAsyncTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _mapperMock = new Mock<IMapper>();
        _userContextServiceMock = new Mock<IUserContextService>();
    }
    
    [Fact]
    public async Task GetCurrentUserIngredientAsync_ShouldReturnsUserAndGlobalIngredients()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userIngredients = new List<Ingredient>
        {
            new Ingredient { UserId = userId, Name = "Salt" },
            new Ingredient { UserId = userId, Name = "Sugar" }
        };
        var globalIngredient = new Ingredient { Name = "Water", UserId = null };

        var ingredients = userIngredients.Append(globalIngredient).ToList();
        
        _ingredientRepositoryMock.Setup(r => r.GetByFilterAsync(
                It.IsAny<Expression<Func<Ingredient, bool>>>(),
                false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredients);

        _mapperMock.Setup(m => m.Map<List<IngredientDto>>(It.IsAny<List<Ingredient>>()))
            .Returns<List<Ingredient>>(src =>
                src.Select(i => new IngredientDto { Id = i.Id, Name = i.Name }).ToList());
        
        _userContextServiceMock.Setup(u => u.GetUserIdOrThrow()).Returns(userId);
        
        var ingredientService = new IngredientService(
            _ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var result = await ingredientService.GetCurrentUserIngredientsAsync(CancellationToken.None);
        
        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(i => i.Name == "Salt");
        result.Should().Contain(i => i.Name == "Sugar");
        result.Should().Contain(i => i.Name == "Water");
    }
}