using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Contracts.Authentications;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers.Authentication;
[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtOptions _jwtOptions;
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, IOptions<JwtOptions> jwtOptions)
    {
        _authService = authService;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem(statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> GenerateRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.GenerateRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem(statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return isRevoked.IsSuccess
        ? Ok("refresh token is revoked")
            : isRevoked.ToProblem(statusCode: StatusCodes.Status400BadRequest);
    }
}
