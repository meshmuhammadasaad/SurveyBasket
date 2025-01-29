using FluentValidation;

namespace SurveyBasket.Api.Contracts.Users;

public record UserProfileUpdateRequest(
    string FirstName,
    string LastName
);

public class UserProfileUpdateRequestValidator : AbstractValidator<UserProfileUpdateRequest>
{
    public UserProfileUpdateRequestValidator()
    {
        RuleFor(c => c.FirstName)
            .Length(3, 100)
            .NotEmpty();

        RuleFor(c => c.LastName).NotEmpty()
            .Length(3, 100);
    }
}
