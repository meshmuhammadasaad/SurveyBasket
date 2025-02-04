using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Authentication;

public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissins);
    string? ValidateToken(string token);
}
