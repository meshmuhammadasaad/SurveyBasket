using Hangfire;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Polls;
using SurveyBasket.Api.Entities;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class PollService(ApplicationDbContext context, INotificationService notificationService) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<IEnumerable<PollResponse>?> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Polls.AsNoTracking()
            .ProjectToType<PollResponse>()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<PollResponse>?> GetCurrentAsync(CancellationToken cancellationToken = default)
        => await _context.Polls
            .Where(c => c.IsPublished && c.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow) && c.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow))
            .AsNoTracking()
            .ProjectToType<PollResponse>()
            .ToListAsync(cancellationToken);

    public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Polls.FindAsync(id, cancellationToken);

        return result is not null
            ? Result.Success(result.Adapt<PollResponse>())
            : Result.Failure<PollResponse>(PollErrors.PollNotFound);
    }

    public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
    {
        var titleExists = await _context.Polls.AnyAsync(t => t.Title == request.Title, cancellationToken: cancellationToken);

        if (titleExists)
            return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

        var poll = request.Adapt<Poll>();

        await _context.Polls.AddAsync(poll, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(poll.Adapt<PollResponse>());
    }
    public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
    {
        var oldPoll = await _context.Polls.FindAsync(id, cancellationToken);

        if (oldPoll is null)
            return Result.Failure(PollErrors.PollNotFound);

        var titleExists = await _context.Polls.AnyAsync(t => t.Title == request.Title && t.Id != id, cancellationToken: cancellationToken);

        if (titleExists)
            return Result.Failure(PollErrors.DuplicatedPollTitle);

        oldPoll.Id = id;
        oldPoll.Title = request.Title;
        oldPoll.Summary = request.Summary;
        oldPoll.StartsAt = request.StartsAt;
        oldPoll.EndsAt = request.EndsAt;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        _context.Polls.Remove(poll);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var Poll = await _context.Polls.FindAsync(id, cancellationToken);

        if (Poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        Poll.IsPublished = !Poll.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);

        if (Poll.IsPublished && Poll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
            BackgroundJob.Enqueue(() => _notificationService.SendNewPollsNotificationAsync(Poll.Id));

        return Result.Success();
    }
}
