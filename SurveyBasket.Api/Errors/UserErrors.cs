using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public static class UserErrors
{
    public static readonly Error InvaildCredentials =
        new("User.InvaildCredentials", "Invalid password or email", StatusCodes.Status401Unauthorized);

    public static readonly Error InvaildJwtToken =
        new("User.InvaildJwtToken", "Invalid JwtToken", StatusCodes.Status401Unauthorized);

    public static readonly Error InvaildRefreshToken =
        new("User.InvaildRefreshToken", "Invalid RefreshToken", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed =
    new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

    public static readonly Error InvaildCode =
    new("User.InvaildCode", "Invaild code", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedEmailConf =
    new("User.DuplicatedEmailConf", "Email is already confirmed", StatusCodes.Status400BadRequest);
}
