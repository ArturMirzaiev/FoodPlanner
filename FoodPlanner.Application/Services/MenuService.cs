using AutoMapper;
using AutoMapper.QueryableExtensions;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;

    public MenuService(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<MenuDto?> GetActiveMenuAsync(Guid personId, CancellationToken cancellationToken = default)
    {
        var query = _menuRepository.GetActiveMenuQuery(personId);

        var menuDto = await query
            .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        
        return menuDto;
    }
}
