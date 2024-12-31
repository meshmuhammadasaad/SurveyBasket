namespace SurveyBasket.Api.Contracts.Authentications;

public record RefreshTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
