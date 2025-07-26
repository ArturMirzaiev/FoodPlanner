/*using AutoMapper;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Application.Mapping;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Exceptions;
using FoodPlanner.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FoodPlanner.Tests.Features.Ingredients;

public class IngredientServiceTests
{
    private readonly Mock<IIngredientRepository> _repoMock = new();
    private readonly IIngredientService _service;

    public IngredientServiceTests()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MenuMappingProfile>();
        }, loggerFactory);

        _service = new IngredientService(_repoMock.Object, new Mapper(config));
    }

    [Fact]
    public async Task UpdateAsync_IngredientNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dto = new UpdateIngredientDto { Id = Guid.NewGuid(), Name = "New Name" };
        _repoMock.Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Ingredient?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(dto, Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_SystemIngredient_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new UpdateIngredientDto { Id = Guid.NewGuid(), Name = "New Name" };
        var ingredient = new Ingredient { Id = dto.Id, UserId = null }; 
        _repoMock.Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(ingredient);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _service.UpdateAsync(dto, Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_ValidIngredient_UpdatesAndSaves()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dto = new UpdateIngredientDto { Id = Guid.NewGuid(), Name = "Updated Name" };
        var ingredient = new Ingredient { Id = dto.Id, UserId = userId, Name = "Old Name" };
        
        _repoMock.Setup(r => r.GetByIdAsync(dto.Id, userId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(ingredient);
        _repoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask)
                 .Verifiable();

        // Act
        await _service.UpdateAsync(dto, userId, CancellationToken.None);

        // Assert
        Assert.Equal(dto.Name, ingredient.Name);
        _repoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task DeleteAsync_IngredientNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ingredient?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(id, userId, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_SystemIngredient_ThrowsForbiddenException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Ingredient { Id = id, UserId = null });

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _service.DeleteAsync(id, userId, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_ValidIngredient_DeletesAndSaves()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var ingredient = new Ingredient { Id = id, UserId = userId, Name = "Old Name" };

        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredient);
        _repoMock.Setup(r => r.Remove(It.IsAny<Ingredient>())).Verifiable();
        _repoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await _service.DeleteAsync(id, userId, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.Remove(ingredient), Times.Once);
        _repoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}*/