using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Abstractions.Consts;
using SurveyBasket.Api.Contracts.Polls;
using SurveyBasket.Api.Filters;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    [HasPermission(Permissions.GetPolls)] 
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync(cancellationToken);

        return Ok(polls);
    }

    [Authorize(Roles = DefaultRoles.Member)]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetCurrentAsync(cancellationToken);

        return Ok(polls);
    }

    [HasPermission(Permissions.GetPolls)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.GetByIdAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HasPermission(Permissions.AddPolls)]
    [HttpPost("")]
    public async Task<IActionResult> Add(PollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : result.ToProblem();
    }

    [HasPermission(Permissions.UpdatePolls)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HasPermission(Permissions.DeletePolls)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HasPermission(Permissions.UpdatePolls)]
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
