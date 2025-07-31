using AutoMapper;
using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Services.Contracts;
using MediatR;

namespace FoodPlanner.Application.Authentication.Features.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    
    public LoginUserCommandHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var loginDto = _mapper.Map<LoginDto>(request);
        var result = await _authService.LoginAsync(loginDto, cancellationToken);

        return result;
    }
}