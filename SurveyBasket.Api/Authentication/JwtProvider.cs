using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Api.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SurveyBasket.Api.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissins)
    {
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub,user.Id),
            new(JwtRegisteredClaimNames.Email,user.Email!),
            new(JwtRegisteredClaimNames.GivenName,user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName,user.LastName),
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new(nameof(roles),JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray),
            new(nameof(permissins),JsonSerializer.Serialize(permissins),JsonClaimValueTypes.JsonArray)
            ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var signingCardintial = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                signingCredentials: signingCardintial,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes)
            );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);


        return (token: token, expiresIn: 30);
    }

    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = jwtToken.Claims.First(t => t.Type == JwtRegisteredClaimNames.Sub).Value;

            return userId;
        }
        catch
        {

            return null;
        }

    }
}
