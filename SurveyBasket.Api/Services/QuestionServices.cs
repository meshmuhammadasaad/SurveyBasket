using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Questions;
using SurveyBasket.Api.Entities;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class QuestionServices(ApplicationDbContext context) : IQuestionServices
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(c => c.Id == pollId, cancellationToken);

        if (!pollIsExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

        var questions = await _context.Questions
            .Where(q => q.PollId == pollId)
            .Include(a => a.Answers)
            //.Select(q => new QuestionResponse(
            //    q.Id,
            //    q.Content,
            //    q.Answers.Select(a => new AnswerResponse(a.Id, a.Content))
            //    ))
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(questions.Adapt<IEnumerable<QuestionResponse>>());

    }

    public async Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .Where(q => q.PollId == pollId && q.Id == id)
            .Include(a => a.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

        return Result.Success(question);
    }

    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(c => c.Id == pollId, cancellationToken);

        if (!pollIsExists)
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

        var questionsIsExists = await _context.Questions.AnyAsync(c => c.Content == request.Content && c.PollId == pollId, cancellationToken);

        if (questionsIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

        var question = request.Adapt<Question>();
        question.PollId = pollId;

        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());
    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var contentIsExists = await _context.Questions
            .AnyAsync(c => c.PollId == pollId && c.Content == request.Content && c.Id != id, cancellationToken);

        if (contentIsExists)
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

        var question = await _context.Questions
            .Include(c => c.Answers)
            .FirstOrDefaultAsync(c => c.PollId == pollId && c.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);


        question.Content = request.Content;

        var currentAnswers = question.Answers.Select(a => a.Content).ToList();

        var newanswers = request.Answers.Except(currentAnswers).ToList();

        newanswers.ForEach(answer =>
        {
            question.Answers.Add(new Answer { Content = answer });
        });

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.Answers.Contains(answer.Content);
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions.SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
