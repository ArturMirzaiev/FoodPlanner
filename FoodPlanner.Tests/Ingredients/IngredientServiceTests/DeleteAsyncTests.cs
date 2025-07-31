using AutoMapper;
using FluentAssertions;
using FoodPlanner.Application.Shared.Services;
using FoodPlanner.Domain.Core.Constants;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Exceptions;
using FoodPlanner.Domain.Interfaces;
using FoodPlanner.Infrastructure.Services;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Ingredients.IngredientServiceTests;

public class DeleteAsyncTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    
    public DeleteAsyncTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _userContextServiceMock = new Mock<IUserContextService>();
        _mapperMock = new Mock<IMapper>();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var act = () =>  ingredientService.DeleteAsync(Guid.Empty);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Ingredient id cannot be empty*");
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFound_WhenIngredientNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _ingredientRepositoryMock
            .Setup(r => r.GetByIdAsync(id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ingredient?)null);

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var act = () => ingredientService.DeleteAsync(id);

        // Assert
        var exception = await act.Should().ThrowAsync<IngredientException>();
        exception.Which.SubCode.Should().Be(SubCodes.Ingredient.NotFound);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowIsReadOnly_WhenUserIdIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ingredient = new Ingredient { Id = id, UserId = null };

        _ingredientRepositoryMock
            .Setup(r => r.GetByIdAsync(id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);

        _userContextServiceMock
            .Setup(s => s.GetUserIdOrThrow())
            .Returns(Guid.NewGuid());

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var act = () => ingredientService.DeleteAsync(id);

        // Assert
        var exception = await act.Should().ThrowAsync<IngredientException>();
        exception.Which.SubCode.Should().Be(SubCodes.Ingredient.IsReadOnly);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowNotOwned_WhenUserIdMismatch()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var ingredient = new Ingredient { Id = id, UserId = Guid.NewGuid() };

        _ingredientRepositoryMock
            .Setup(r => r.GetByIdAsync(id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);

        _userContextServiceMock
            .Setup(s => s.GetUserIdOrThrow())
            .Returns(userId);

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var act = () => ingredientService.DeleteAsync(id);

        // Assert
        var exception = await act.Should().ThrowAsync<IngredientException>();
        exception.Which.SubCode.Should().Be(SubCodes.Ingredient.NotOwned);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAndSave_WhenValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var ingredient = new Ingredient { Id = id, UserId = userId };

        _ingredientRepositoryMock
            .Setup(r => r.GetByIdAsync(id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);

        _userContextServiceMock
            .Setup(s => s.GetUserIdOrThrow())
            .Returns(userId);

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        await ingredientService.DeleteAsync(id);

        // Assert
        _ingredientRepositoryMock.Verify(r => r.Remove(ingredient), Times.Once);
        _ingredientRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}