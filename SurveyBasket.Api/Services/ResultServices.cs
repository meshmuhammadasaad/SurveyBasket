using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Results;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class ResultServices(ApplicationDbContext context) : IResultServices
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollVotes = await _context.Polls
            .Where(c => c.Id == pollId)
            .Select(c => new PollVotesResponse(
                  c.Title,
                  c.Votes.Select(v => new VoteRsponse(
                      $"{v.User.FirstName} {v.User.LastName}",
                      v.SubmittedOn, v.VoteAnswers.Select(q => new QuestionAnswerResponse(
                          q.Question.Content, q.Answer.Content
                      ))
                  ))
            ))
            .SingleOrDefaultAsync(cancellationToken);

        return pollVotes is null
             ? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound)
             : Result.Success(pollVotes);
    }

    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(c => c.Id == pollId, cancellationToken);

        if (!pollIsExists)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);

        var votesPerDay = await _context.Votes
            .Where(c => c.PollId == pollId)
            .GroupBy(c => new { Date = DateOnly.FromDateTime(c.SubmittedOn) })
            .Select(c => new VotesPerDayResponse(
                c.Key.Date,
                c.Count()
             ))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);
    }

    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerAnswersAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(c => c.Id == pollId, cancellationToken);

        if (!pollIsExists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);

        var VotesPerQuestion = await _context.VoteAnswers
            .Where(c => c.Vote.PollId == pollId)
            .Select(c => new VotesPerQuestionResponse(
                c.Question.Content,
                c.Question.VoteAnswers
                    .GroupBy(c => new { AnswerId = c.Answer.Id, AnswerContent = c.Answer.Content })
                    .Select(c => new VotesPerAnswersResponse(
                        c.Key.AnswerContent,
                        c.Count()
                    ))
            ))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(VotesPerQuestion);
    }
}
