using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Votes;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class VoteServices(ApplicationDbContext context) : IVoteServices
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
    {
        var hasVote = await _context.Votes.AnyAsync(c => c.PollId == pollId && c.UserId == userId, cancellationToken);

        if (hasVote)
            return Result.Failure(VoteErrors.DuplicatedVote);

        var pollIsExists = await _context.Polls.AnyAsync(c => c.Id == pollId && c.IsPublished
        && c.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && c.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow));

        if (!pollIsExists)
            return Result.Failure(PollErrors.PollNotFound);


    }
}
