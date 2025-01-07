namespace SurveyBasket.Api.Contracts.Results;

public record PollVotesResponse(
        string Title,
        IEnumerable<VoteRsponse> Votes
    );
