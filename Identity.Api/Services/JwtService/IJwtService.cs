using Identity.Api.Models.ServiceData.Tokens;
using Identity.Api.Models.ServiceData.UserData;

namespace Identity.Api.Services.JwtService
{
    public interface IJwtService
    {
        string GenerateJwtToken(Account account);

        string? ValidateJwtToken(string token);

        RefreshToken GenerateRefreshToken(Account account, string ipAddress);
    }
}
