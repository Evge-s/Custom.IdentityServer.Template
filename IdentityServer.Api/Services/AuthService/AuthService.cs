using IdentityServer.Api.Models.CustomErrors;
using IdentityServer.Api.Models.Data.Tokens;
using IdentityServer.Api.Models.Data.UserData;
using IdentityServer.Api.Models.ServiceData;
using IdentityServer.Services.JwtService;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace IdentityServer.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly ServiceContext _serviceContext;
        private readonly IJwtService _jwtService;

        public AuthService(ILogger<AuthService> logger, IJwtService jwtService, ServiceContext serviceContext)
        {
            _logger = logger;
            _jwtService = jwtService;
            _serviceContext = serviceContext;
        }

        public async Task<bool> Register(string email, string password)
        {
            if (await UserExist(email))
                return false;

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var acc = new Account
            {
                Id = Guid.NewGuid(),
                Login = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow,
                RefreshTokens = new List<RefreshToken>()
            };

            _serviceContext.Accounts.Add(acc);
            await _serviceContext.SaveChangesAsync();

            return true;
        }

        public async Task<(string, string)> Login(string email, string password)
        {
            var acc = await _serviceContext.Accounts.FirstOrDefaultAsync(u => u.Login.ToLower().Equals(email.ToLower()));

            if (acc is null)
            {
                throw new UserNotFoundException("User not found");
            }
            else if (!VerifyPasswordHash(password, acc.PasswordHash, acc.PasswordSalt))
            {
                throw new InvalidPasswordException("Invalid password");
            }
            else
            {
                var jwtToken = _jwtService.GenerateJwtToken(acc);
                var refreshToken = _jwtService.GenerateRefreshToken(acc, "ip");
                acc.RefreshTokens.Add(refreshToken);

                await _serviceContext.SaveChangesAsync();

                return (jwtToken, refreshToken.Token);
            }
        }

        private async Task<bool> UserExist(string email)
        {
            if (await _serviceContext.Accounts.AnyAsync(a => a.Login.ToLower()
                .Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
