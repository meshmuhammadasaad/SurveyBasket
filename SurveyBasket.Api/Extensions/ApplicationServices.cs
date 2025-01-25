using Microsoft.AspNetCore.Identity.UI.Services;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Services;
using SurveyBasket.Api.Services.Caching;

namespace SurveyBasket.Api.Extensions;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQuestionServices, QuestionServices>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddScoped<IVoteServices, VoteServices>();
        services.AddScoped<IResultServices, ResultServices>();
        services.AddScoped<ICacheServices, CacheServices>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddHttpContextAccessor();

        // middleware setting
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();


        return services;
    }
}
