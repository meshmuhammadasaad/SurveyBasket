namespace SurveyBasket.Api.Services;

public interface INotificationService
{
    Task SendNewPollsNotificationAsync(int? pollId = null);
}
