using FluentValidation;

namespace SurveyBasket.Api.Contracts.Votes;

public record VoteAnswerRequest(int QuestionId, int AnswerId);

public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
{
    public VoteAnswerRequestValidator()
    {
        RuleFor(c => c.QuestionId).GreaterThan(0);
        RuleFor(c => c.AnswerId).GreaterThan(0);
    }
}