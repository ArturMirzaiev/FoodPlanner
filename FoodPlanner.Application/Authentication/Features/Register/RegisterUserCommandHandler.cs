using AutoMapper;
using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Services.Contracts;
using MediatR;

namespace FoodPlanner.Application.Authentication.Features.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    
    public RegisterUserCommandHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerDto = _mapper.Map<RegisterDto>(request);

        return await _authService.RegisterAsync(registerDto, cancellationToken);
    }
}