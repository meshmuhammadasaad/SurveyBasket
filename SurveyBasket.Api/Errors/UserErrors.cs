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
}
