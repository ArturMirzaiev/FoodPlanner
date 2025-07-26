using AutoMapper;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Responses;
using MediatR;

namespace FoodPlanner.Features.Authentication.Commands;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginResponseDto>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    
    public LoginUserCommandHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var loginDto = _mapper.Map<LoginDto>(request);
        var result = await _authService.LoginAsync(loginDto, cancellationToken);

        return result;
    }
}