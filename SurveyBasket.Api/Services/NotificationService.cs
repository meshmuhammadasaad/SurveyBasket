
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Entities;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class NotificationService(ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    IEmailSender emailSender) : INotificationService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task SendNewPollsNotificationAsync(int? pollId = null)
    {
        IEnumerable<Poll> polls = [];

        if (pollId.HasValue)
        {
            var poll = await _context.Polls.SingleOrDefaultAsync(c => c.Id == pollId && c.IsPublished);

            polls = [poll!];
        }
        else
        {
            polls = await _context.Polls
                .Where(c => c.IsPublished && c.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                .AsNoTracking()
                .ToListAsync();
        }

        //todo select Member only

        var users = await _userManager.Users.ToListAsync();

        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        foreach (var poll in polls)
        {
            foreach (var user in users)
            {
                var placeHolder = new Dictionary<string, string>
                {
                    {"{{name}}" ,user.FirstName},
                    {"{{pollTitle}}" ,poll.Title},
                    {"{{endDate}}" ,poll.EndsAt.ToString()},
                    {"{{url}}" ,$"{origin}/polls/start/{poll.Id}"},
                };

                await _emailSender.SendEmailAsync(user.Email!, "Survey Basket: New Polls", placeHolder.ToString()!);
            }
        }
    }
}
