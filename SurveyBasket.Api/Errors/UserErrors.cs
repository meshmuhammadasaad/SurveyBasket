using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public static class UserErrors
{
    public static readonly Error InvaildCredentials =
        new("User.InvaildCredentials", "Invalid password or email");
    public static readonly Error InvaildJwtToken =
        new("User.InvaildJwtToken", "Invalid JwtToken");
    public static readonly Error InvaildRefreshToken =
        new("User.InvaildRefreshToken", "Invalid RefreshToken");
}
