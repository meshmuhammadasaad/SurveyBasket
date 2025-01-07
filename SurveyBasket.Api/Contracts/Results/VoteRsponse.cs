namespace SurveyBasket.Api.Contracts.Results;

public record VoteRsponse(
    string VoterName,
    DateTime VoteDate,
    IEnumerable<QuestionAnswerResponse> SelectedAnswers
);