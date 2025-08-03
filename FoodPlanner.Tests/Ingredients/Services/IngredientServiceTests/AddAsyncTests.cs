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

public class AddAsyncTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    
    public AddAsyncTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _mapperMock = new Mock<IMapper>();
        _userContextServiceMock = new Mock<IUserContextService>();
    }

    [Fact]
    public async Task AddAsync_ShouldCreateIngredientAndReturnDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createDto = new CreateIngredientDto
        {
            Name = "Salt"
        };

        var ingredient = new Ingredient
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            UserId = userId
        };

        var ingredientDto = new IngredientDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name
        };

        _userContextServiceMock.Setup(x => x.GetUserIdOrThrow()).Returns(userId);
        _mapperMock.Setup(x => x.Map<Ingredient>(createDto)).Returns(ingredient);
        _mapperMock.Setup(x => x.Map<IngredientDto>(ingredient)).Returns(ingredientDto);

        var service = new IngredientService(_ingredientRepositoryMock.Object, _mapperMock.Object, _userContextServiceMock.Object);

        // Act
        var result = await service.AddAsync(createDto);

        // Assert
        result.Should().BeEquivalentTo(ingredientDto);

        _userContextServiceMock.Verify(x => x.GetUserIdOrThrow(), Times.Once);
        _mapperMock.Verify(x => x.Map<Ingredient>(createDto), Times.Once);
        _ingredientRepositoryMock.Verify(x => x.CreateAsync(ingredient, It.IsAny<CancellationToken>()), Times.Once);
        _ingredientRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.Map<IngredientDto>(ingredient), Times.Once);
    }
}