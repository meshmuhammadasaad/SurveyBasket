using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Models;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
    private readonly IPollService _pollService;

    public PollsController(IPollService pollService)
    {
        _pollService = pollService;
    }

    [HttpGet("")]
    public IActionResult GetAll()
    {
        var polls = _pollService.GetAll();
        return Ok(polls);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var poll = _pollService.GetById(id);

        return poll is null ? NotFound() : Ok(poll);
    }

    [HttpPost("")]
    public IActionResult Add(Poll request)
    {
        var newPoll = _pollService.Add(request);

        return CreatedAtAction(nameof(GetById), new { id = newPoll.Id }, newPoll);
    }
}
