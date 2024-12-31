namespace SurveyBasket.Api.Contracts.Authentications;

public record LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
