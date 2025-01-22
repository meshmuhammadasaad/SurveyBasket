using FluentValidation;

namespace SurveyBasket.Api.Contracts.Authentications;

public record ResendConfirmationEmailRequest(
    string Email
);



public class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequest>
{
    public ResendConfirmationEmailRequestValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();
    }
}