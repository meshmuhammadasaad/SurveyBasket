namespace SurveyBasket.Api.Contracts.Authentications;

public record ConfirmEmailRequest(
    string UserId,
    string Code
);
