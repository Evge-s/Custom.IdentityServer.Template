using System.Security.Cryptography;
using Identity.Api.Models.CustomErrors;
using Identity.Api.Models.ServiceData;
using Identity.Api.Models.ServiceData.Tokens;
using Identity.Api.Models.ServiceData.UserData;
using Identity.Api.Services.EmailService;
using Identity.Api.Services.JwtService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Api.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ServiceContext _serviceContext;

        public AuthService(
            ILogger<AuthService> logger,
            IJwtService jwtService,
            IEmailService emailService,
            ServiceContext serviceContext)
        {
            _logger = logger;
            _jwtService = jwtService;
            _emailService = emailService;
            _serviceContext = serviceContext;
        }

        public async Task<bool> ConfirmEmail(string email, int code)
        {
            return await _serviceContext.ConfirmationEmails
                .AnyAsync(e => e.Email == email && e.Code == code && e.IsActive);
        }
        
        public async Task<bool> RegisterByEmail(string email, string password)
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
            await CleanConfirmedEmailCodes(email);
            
            return true;
        }

        public async Task<(string, string)> LoginByEmail(string email, string password)
        {
            var acc = await _serviceContext.Accounts.FirstOrDefaultAsync(u =>
                u.Login.ToLower().Equals(email.ToLower()));

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
        
        private async Task CleanConfirmedEmailCodes(string email)
        {
            var confirmationsToDelete =
                await _serviceContext.ConfirmationEmails.Where(e => e.Email == email).ToListAsync();

            _serviceContext.ConfirmationEmails.RemoveRange(confirmationsToDelete);

            await _serviceContext.SaveChangesAsync();
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