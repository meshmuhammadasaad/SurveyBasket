using FluentValidation;

namespace SurveyBasket.Api.Contracts.Authentications;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();

        RuleFor(c => c.Code).NotEmpty();
    }
}
