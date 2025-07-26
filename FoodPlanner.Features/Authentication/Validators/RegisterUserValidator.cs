using FluentValidation;
using FoodPlanner.Features.Authentication.Commands;

namespace FoodPlanner.Features.Authentication.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 20).WithMessage("Username must be between 3 and 20 characters")
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Username can only contain letters and numbers");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .Length(5, 20).WithMessage("Password must be between 5 and 20 characters");
    }
}
