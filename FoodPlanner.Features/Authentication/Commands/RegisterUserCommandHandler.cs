using AutoMapper;
using Azure.Core;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Core.Common;
using MediatR;

namespace FoodPlanner.Features.Authentication.Commands;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterResponseDto>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    
    public RegisterUserCommandHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<Result<RegisterResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerDto = _mapper.Map<RegisterDto>(request);

        var result = await _authService.RegisterAsync(registerDto, cancellationToken);

        return result;
    }
}