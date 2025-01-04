using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public class VoteErrors
{
    public static readonly Error InvalidQuestions =
    new("Vote.InvalidQuestions", "Invalid questions", StatusCodes.Status400BadRequest);
    public static readonly Error DuplicatedVote =
        new("Vote.DuplicatedVote", "This user has already voted on this vote", StatusCodes.Status409Conflict);
}
