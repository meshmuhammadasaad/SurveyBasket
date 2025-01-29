using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Contracts.Authentications;
using SurveyBasket.Api.Entities;
using SurveyBasket.Api.Errors;
using System.Security.Cryptography;
using System.Text;

namespace SurveyBasket.Api.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider,
    ILogger<AuthService> logger,
    IHttpContextAccessor _httpContextAccessor,
    IEmailSender emailSender) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = _httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(c => c.Email == request.Email, cancellationToken);

        if (emailIsExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.Email;

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.First();
            return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var scheme = _httpContextAccessor.HttpContext?.Request.Scheme;
        var origin = _httpContextAccessor.HttpContext?.Request.Host.Value;

        var callbackUrl = $"{scheme}://{origin}/auth/confirm-email?userId={user.Id}&code={code}";

        _logger.LogInformation("Confirmation Email Url : {callbackUrl}", callbackUrl);

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Email Confirmation", $"Please confirm your email by clicking :   {callbackUrl} "));

        //await _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Email Confirmation", $"Please confirm your email by clicking :   {callbackUrl} ");

        return Result.Success();
    }

    public async Task<Result> ConfiremEmailAsync(ConfirmEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvaildCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedEmailConf);

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (Exception)
        {
            return Result.Failure(UserErrors.InvaildCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (!result.Succeeded)
        {
            var errors = result.Errors.First();
            return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
        }

        return Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedEmailConf);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var scheme = _httpContextAccessor.HttpContext?.Request.Scheme;
        var origin = _httpContextAccessor.HttpContext?.Request.Host.Value;

        var callbackUrl = $"{scheme}://{origin}/auth/confirm-email?userId={user.Id}&code={code}";

        _logger.LogInformation("Confirmation Email Url : {callbackUrl}", callbackUrl);

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Email Confirmation", $"Please confirm your email by clicking: {callbackUrl}"));

        //await _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Email Confirmation", $"Please confirm your email by clicking: {callbackUrl}");


        return Result.Success();
    }

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvaildCredentials);

        //var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        if (!result.Succeeded)
            return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvaildCredentials);

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            token,
            expiresIn,
            refreshToken,
            refreshTokenExpiration
        );

        return Result.Success(response);
    }

    public async Task<Result<AuthResponse>> GenerateRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvaildJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvaildCredentials);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken && r.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvaildRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse
        (
             user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            newToken,
            expiresIn,
            newRefreshToken,
            refreshTokenExpiration
        );

        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvaildJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvaildCredentials);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken && r.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvaildRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> SendResetPasswordCodeAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed);

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var scheme = _httpContextAccessor.HttpContext?.Request.Scheme;
        var origin = _httpContextAccessor.HttpContext?.Request.Host.Value;

        var callbackUrl = $"{scheme}://{origin}/auth/reset-password?email={user.Email}&code={code}";

        _logger.LogInformation("Reset Password : {callbackUrl}", callbackUrl);

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survey Basket: change password", $"Please confirm your email by clicking :   {callbackUrl} "));

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.InvaildCode);

        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
        }

        return Result.Success();
    }
    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
