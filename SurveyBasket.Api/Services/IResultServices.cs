﻿using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Results;

namespace SurveyBasket.Api.Services;

public interface IResultServices
{
    Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerAnswersAsync(int pollId, CancellationToken cancellationToken = default);

}
