using FluentValidation;

namespace SurveyBasket.Api.Contracts.Votes;

public record VoteRequest(
    IEnumerable<VoteAnswerRequest> Answers
);


public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(c => c.Answers)
            .NotEmpty();
    }
}