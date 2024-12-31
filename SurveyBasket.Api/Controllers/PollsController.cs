using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Polls;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController : ControllerBase
{
    private readonly IPollService _pollService;

    public PollsController(IPollService pollService)
    {
        _pollService = pollService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync(cancellationToken);

        return Ok(polls);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.GetByIdAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem(statusCode: StatusCodes.Status404NotFound);

    }

    [HttpPost("")]
    public async Task<IActionResult> Add(PollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : result.ToProblem(statusCode: StatusCodes.Status409Conflict);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return result.Error.Code == PollErrors.PollNotFound.Code
             ? result.ToProblem(statusCode: StatusCodes.Status404NotFound)
             : result.ToProblem(statusCode: StatusCodes.Status409Conflict);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem(statusCode: StatusCodes.Status404NotFound);
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem(statusCode: StatusCodes.Status404NotFound);
    }

}
