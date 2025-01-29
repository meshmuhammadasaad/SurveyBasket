using FluentValidation;
using SurveyBasket.Api.Abstractions.Consts;

namespace SurveyBasket.Api.Contracts.Authentications;

public record ResetPasswordRequest(
    string Email,
    string Code,
    string NewPassword
);

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(c => c.Email).NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Code)
            .NotEmpty();

        RuleFor(r => r.NewPassword)
         .NotEmpty()
         .Matches(RegexPatterns.Password)
         .WithMessage("password shoud be at least 8 digits and shoud contains Lowercase, NonAlphanumeric and Uppercase");

    }
}
