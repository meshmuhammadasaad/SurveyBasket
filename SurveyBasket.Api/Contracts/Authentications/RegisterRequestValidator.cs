using FluentValidation;
using SurveyBasket.Api.Abstractions.Consts;

namespace SurveyBasket.Api.Contracts.Authentications;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty()
            .EmailAddress();

        RuleFor(r => r.Password).NotEmpty()
            .MinimumLength(8)
            .Matches(RegexPatterns.Password)
            .WithMessage("password shoud be at least 8 digits and shoud contains Lowercase, NonAlphanumeric and Uppercase");

        RuleFor(r => r.FirstName).NotEmpty()
            .Length(3, 100);

        RuleFor(r => r.LastName).NotEmpty()
            .Length(3, 100);
    }
}
