﻿using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Polls;

namespace SurveyBasket.Api.Services;

public interface IPollService
{
    Task<IEnumerable<PollResponse>?> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PollResponse>?> GetCurrentAsync(CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
}
