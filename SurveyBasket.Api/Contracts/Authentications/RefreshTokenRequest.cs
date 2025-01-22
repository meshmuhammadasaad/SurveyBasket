namespace SurveyBasket.Api.Contracts.Authentications;

public record RefreshTokenRequest(
     string Token,
     string RefreshToken
);
