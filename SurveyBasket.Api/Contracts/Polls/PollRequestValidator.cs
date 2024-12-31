using FluentValidation;

namespace SurveyBasket.Api.Contracts.Polls;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
    public PollRequestValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(p => p.Summary)
            .NotEmpty()
            .Length(3, 1500);

        RuleFor(p => p.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(p => p.EndsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(p => p.StartsAt);
    }
}
