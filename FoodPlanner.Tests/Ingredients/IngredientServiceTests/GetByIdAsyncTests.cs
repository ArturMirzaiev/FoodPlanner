using AutoMapper;
using FluentAssertions;
using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Shared.Services;
using FoodPlanner.Domain.Core.Constants;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Exceptions;
using FoodPlanner.Domain.Interfaces;
using FoodPlanner.Infrastructure.Services;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Ingredients.IngredientServiceTests;

public class GetByIdAsyncTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    
    public GetByIdAsyncTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _mapperMock = new Mock<IMapper>();
        _userContextServiceMock = new Mock<IUserContextService>();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedDto_WhenIngredientExistsAndOwnedByUser()
    {
        // Arrange
        var ingredientId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var ingredient = new Ingredient
        {
            Id = ingredientId,
            Name = "Salt",
            UserId = userId
        };

        var expectedDto = new IngredientDto
        {
            Id = ingredientId,
            Name = "Salt"
        };

        _ingredientRepositoryMock.Setup(r => r.GetByIdAsync(ingredientId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);

        _mapperMock.Setup(m => m.Map<IngredientDto>(ingredient))
            .Returns(expectedDto);

        _userContextServiceMock.Setup(u => u.GetUserIdOrThrow()).Returns(userId);

        var service = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);

        // Act
        var result = await service.GetByIdAsync(ingredientId);

        // Assert
        result.Should().BeEquivalentTo(expectedDto);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        var id = Guid.Empty;
        var service = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);

        // Act
        var act = async () => await service.GetByIdAsync(id);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .Where(e => e.ParamName == "id" && e.Message.Contains("Ingredient id cannot be empty"));
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFound_WhenIngredientDoesNotExist()
    {
        // Arrange
        var ingredientId = Guid.NewGuid();
        
        _ingredientRepositoryMock.Setup(r => r.GetByIdAsync(ingredientId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ingredient?)null);

        var service = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);

        // Act
        var act = async () => await service.GetByIdAsync(ingredientId);

        // Assert
        await act.Should()
            .ThrowAsync<IngredientException>()
            .Where(e => e.SubCode == SubCodes.Ingredient.NotFound);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotOwned_WhenUserIsNotOwner()
    {
        // Arrange
        var ingredientId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var ingredient = new Ingredient
        {
            Id = ingredientId,
            Name = "Oil",
            UserId = otherUserId
        };

        _ingredientRepositoryMock.Setup(r => r.GetByIdAsync(ingredientId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);

        _userContextServiceMock.Setup(u => u.GetUserIdOrThrow()).Returns(ownerId);

        var service = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);

        // Act
        var act = async () => await service.GetByIdAsync(ingredientId);

        // Assert
        await act.Should()
            .ThrowAsync<IngredientException>()
            .Where(e => e.SubCode == SubCodes.Ingredient.NotOwned);
    }
}