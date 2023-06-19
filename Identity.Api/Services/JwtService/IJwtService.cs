using Identity.Models.ServiceData.Tokens;
using Identity.Models.ServiceData.UserData;

namespace Identity.Services.JwtService
{
    public interface IJwtService
    {
        string GenerateJwtToken(Account account);

        string? ValidateJwtToken(string token);

        RefreshToken GenerateRefreshToken(Account account, string ipAddress);
    }
}
