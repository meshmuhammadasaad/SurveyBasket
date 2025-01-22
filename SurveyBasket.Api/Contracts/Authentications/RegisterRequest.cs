namespace SurveyBasket.Api.Contracts.Authentications;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password
);
