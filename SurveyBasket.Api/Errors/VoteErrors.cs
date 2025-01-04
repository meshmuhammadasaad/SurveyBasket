using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public class VoteErrors
{
    //public static readonly Error PollNotFound =
    //new("Poll.Notfound", "No poll was found with the given ID");
    public static readonly Error DuplicatedVote =
        new("Vote.DuplicatedVote", "This user has already voted on this vote");
}
