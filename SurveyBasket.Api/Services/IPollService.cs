using SurveyBasket.Api.Models;

namespace SurveyBasket.Api.Services;

public interface IPollService
{
    IEnumerable<Poll> GetAll();
    Poll? GetById(int id);
    Poll Add(Poll poll);
}
