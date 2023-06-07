using IdentityServer.Api.Models.Data.Tokens;
using IdentityServer.Api.Models.Data.UserData;

namespace IdentityServer.Services.JwtService
{
    public interface IJwtService
    {
        string GenerateJwtToken(Account account);

        string? ValidateJwtToken(string token);

        RefreshToken GenerateRefreshToken(Account account, string ipAddress);
    }
}
