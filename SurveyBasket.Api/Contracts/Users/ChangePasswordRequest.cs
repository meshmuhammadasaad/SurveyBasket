using FluentValidation;
using SurveyBasket.Api.Abstractions.Consts;

namespace SurveyBasket.Api.Contracts.Users;

public record ChangePasswordRequest(string OldPassword, string NewPassword);

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(c => c.OldPassword)
            .NotEmpty();

        RuleFor(r => r.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("password shoud be at least 8 digits and shoud contains Lowercase, NonAlphanumeric and Uppercase")
            .NotEqual(c => c.OldPassword)
            .WithMessage("The new password cannot be the same old password");
    }
}