using FluentValidation;

namespace SurveyBasket.Api.Contracts.Authentications;

public class AuthRequestValidator : AbstractValidator<LoginRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(l => l.Email).NotEmpty().EmailAddress();

        RuleFor(l => l.Password).NotEmpty();
    }
}
