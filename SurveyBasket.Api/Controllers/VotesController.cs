using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Extensions;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;
[Route("api/polls/{pollId}/Vote")]
[ApiController]
[Authorize]
public class VotesController(IQuestionServices questionServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;

    [HttpGet("")]
    public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _questionServices.GetAvailableAsync(pollId, userId!, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return (result.Error == PollErrors.PollNotFound)
            ? result.ToProblem(StatusCodes.Status404NotFound)
            : result.ToProblem(StatusCodes.Status409Conflict);
    }
}
