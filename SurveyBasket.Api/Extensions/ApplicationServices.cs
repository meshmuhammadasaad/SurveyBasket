using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Extensions;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQuestionServices, QuestionServices>();
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();


        return services;
    }
}
