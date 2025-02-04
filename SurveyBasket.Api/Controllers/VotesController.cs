using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Abstractions.Consts;
using SurveyBasket.Api.Contracts.Votes;
using SurveyBasket.Api.Extensions;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;
[Route("api/polls/{pollId}/Vote")]
[ApiController]
[Authorize(Roles = DefaultRoles.Member)] 
public class VotesController(IQuestionServices questionServices, IVoteServices voteServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;
    private readonly IVoteServices _voteServices = voteServices;

    [HttpGet("")]
    public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _questionServices.GetAvailableAsync(pollId, userId!, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _voteServices.AddAsync(pollId, userId!, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
}
