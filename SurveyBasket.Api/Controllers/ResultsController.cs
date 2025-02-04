using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Abstractions.Consts;
using SurveyBasket.Api.Filters;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
//[Authorize(Roles = DefaultRoles.Admin)]
[HasPermission(Permissions.Results)]
public class ResultsController(IResultServices resultServices) : ControllerBase
{
    private readonly IResultServices _resultServices = resultServices;

    [HttpGet("row-data")]
    public async Task<IActionResult> GetPollVots([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _resultServices.GetPollVotesAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("Votes-per-day")]
    public async Task<IActionResult> GetVotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _resultServices.GetVotesPerDayAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("Votes-per-answer")]
    public async Task<IActionResult> GetVotesPerAnswers([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _resultServices.GetVotesPerAnswersAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
