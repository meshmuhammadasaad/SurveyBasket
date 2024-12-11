using SurveyBasket.Api.Models;

namespace SurveyBasket.Api.Services;

public class PollService : IPollService
{
    private static readonly List<Poll> _poll = [
        new Poll{
            Id = 1,
            Title="test1",
            Description = "test1"
            },
        new Poll{
            Id=2,
            Title="test2",
            Description = "test2"
        }
        ];

    public Poll Add(Poll poll)
    {
        poll.Id = _poll.Count + 1;

        _poll.Add(poll);
        return poll;
    }

    public IEnumerable<Poll> GetAll() => _poll;

    public Poll? GetById(int id) => _poll.SingleOrDefault(p => p.Id == id);

}
