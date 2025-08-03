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

namespace FoodPlanner.Tests.Ingredients.Services.IngredientServiceTests;

public class UpdateAsyncTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public UpdateAsyncTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _userContextServiceMock = new Mock<IUserContextService>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenIdIsEmpty()
    {
        // Arrange
        var dto = new UpdateIngredientDto { Id = Guid.Empty };

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var act = () => ingredientService.UpdateAsync(dto);

        // Assert
        var ex = await act.Should().ThrowAsync<ArgumentException>();
        ex.Which.ParamName.Should().Be("Id");
        ex.Which.Message.Should().Contain("Ingredient id cannot be empty.");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNotFound()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateIngredientDto { Id = id };

        _ingredientRepositoryMock
            .Setup(r => r.GetByIdAsync(id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ingredient?)null);

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var act = () => ingredientService.UpdateAsync(dto);

        var ex = await act.Should().ThrowAsync<IngredientException>();
        ex.Which.SubCode.Should().Be(SubCodes.Ingredient.NotFound);
        ex.Which.Message.Should().Contain($"Ingredient with id '{id}' not found.");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenIngredientIsReadOnly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateIngredientDto { Id = id };
        var ingredient = new Ingredient { Id = id, UserId = null };

        _ingredientRepositoryMock
            .Setup(r => r.GetByIdAsync(id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var act = () => ingredientService.UpdateAsync(dto);

        // Assert
        var ex = await act.Should().ThrowAsync<IngredientException>();
        ex.Which.SubCode.Should().Be(SubCodes.Ingredient.IsReadOnly);
        ex.Which.Message.Should().Contain($"Ingredient with id '{id}' is system-defined and cannot be modified.");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNotOwned()
    {
        // Arrange
        var id = Guid.NewGuid();
        var currentUserId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var dto = new UpdateIngredientDto { Id = id };
        var ingredient = new Ingredient { Id = id, UserId = otherUserId };

        _ingredientRepositoryMock
            .Setup(r => r.GetByIdAsync(id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);

        _userContextServiceMock
            .Setup(u => u.GetUserIdOrThrow())
            .Returns(currentUserId);

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);
        
        // Act
        var act = () => ingredientService.UpdateAsync(dto);

        // Assert
        var ex = await act.Should().ThrowAsync<IngredientException>();
        ex.Which.SubCode.Should().Be(SubCodes.Ingredient.NotOwned);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var dto = new UpdateIngredientDto { Id = id, Name = "Updated" };
        var ingredient = new Ingredient { Id = id, UserId = userId, Name = "Old" };
        var updatedDto = new IngredientDto { Id = id, Name = "Updated" };

        _ingredientRepositoryMock
            .Setup(r => r.GetByIdAsync(id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);

        _userContextServiceMock
            .Setup(u => u.GetUserIdOrThrow())
            .Returns(userId);

        _ingredientRepositoryMock
            .Setup(r => r.SaveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mapperMock
            .Setup(m => m.Map(dto, ingredient))
            .Callback<UpdateIngredientDto, Ingredient>((src, dest) =>
            {
                dest.Name = src.Name;
            });

        _mapperMock
            .Setup(m => m.Map<IngredientDto>(ingredient))
            .Returns(updatedDto);

        var ingredientService = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);

        // Act
        var result = await ingredientService.UpdateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedDto);
    }
}