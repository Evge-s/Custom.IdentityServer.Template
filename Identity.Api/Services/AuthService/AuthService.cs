using System.Security.Cryptography;
using System.Text;
using Identity.Api.Models.CustomErrors;
using Identity.Api.Models.ServiceData;
using Identity.Api.Models.ServiceData.Tokens;
using Identity.Api.Models.ServiceData.UserData;
using Identity.Api.Services.EmailService;
using Identity.Api.Services.JwtService;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> SendConfirmationMail(string email)
        {
            var code = _emailService.GenerateConfirmationCode();
            var confirmationLink = _emailService.GenerateConfirmationLink(email, code);
            var subject = $"Confirmation your email {email}";

            var sendResult = await _emailService.SendMailAsync(email, subject, confirmationLink);

            if (!sendResult)
                return false;

            var confirmationEmail = new ConfirmationEmailCode(code, email);
            _serviceContext.ConfirmationEmails.Add(confirmationEmail);
            await _serviceContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmEmail(string email, string code)
        {
            var confirmation = await _serviceContext.ConfirmationEmails
                .FirstOrDefaultAsync(e => e.Email == email && e.Code == code);

            if (confirmation == null || confirmation.IsExpired)
                return false;

            confirmation.Confirmed = true;
            _serviceContext.ConfirmationEmails.Update(confirmation);
            await _serviceContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterByEmail(string email, string password)
        {
            if (await UserExist(email))
                return false;

            // check if the mail is confirmed (need move to cache)
            if (await IsConfirmedEmail(email))
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
            // Save changes will be called inside CleanConfirmedEmailCodes()
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

        public async Task ChangePassword(string accId, string oldPassword, string newPassword)
        {
            var acc = await _serviceContext.Accounts.FindAsync(accId);

            if (acc == null)
                throw new UserNotFoundException("User not found");

            var isValidPass = VerifyPasswordHash(oldPassword, acc.PasswordHash, acc.PasswordSalt);

            if (!isValidPass)
                throw new InvalidPasswordException("Invalid password");

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            acc.PasswordHash = passwordHash;
            acc.PasswordSalt = passwordSalt;
            _serviceContext.Accounts.Update(acc);
            await _serviceContext.SaveChangesAsync();
        }

        public async Task ResetPassword(string email, string resetToken, string newPassword)
        {
            var passwordResetToken = await _serviceContext.ResetPasswordTokens
                .FirstAsync(e => e.Email == email && e.ResetToken == resetToken);

            if (passwordResetToken is { IsExpired: true })
            {
                _serviceContext.ResetPasswordTokens.Remove(passwordResetToken);
                await _serviceContext.SaveChangesAsync();
                throw new TokenExpiredException("The token has expired");
            }

            var acc = await _serviceContext.Accounts.FirstAsync(a => a.Login == email);

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            acc.PasswordHash = passwordHash;
            acc.PasswordSalt = passwordSalt;
            _serviceContext.Accounts.Update(acc);
            _serviceContext.ResetPasswordTokens.Remove(passwordResetToken);
            await _serviceContext.SaveChangesAsync();
        }

        public async Task<bool> UserExist(string email)
        {
            if (await _serviceContext.Accounts.AnyAsync(a => a.Login.ToLower()
                    .Equals(email.ToLower())))
            {
                return true;
            }

            return false;
        }

        private async Task<bool> IsConfirmedEmail(string email)
        {
            return await _serviceContext.ConfirmationEmails
                .AnyAsync(e => e.Email == email && e.Confirmed && e.IsActive);
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

        private static string GenerateRandomPassword()
        {
            const string LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string SpecialCharacters = "!@#$%^&*";
            const string Digits = "1234567890";
            const string AllChars = LowerCaseLetters + SpecialCharacters + UpperCaseLetters + Digits;

            Random rand = new Random();
            int length = rand.Next(8, 13);
            StringBuilder password = new StringBuilder(length);

            password.Append(UpperCaseLetters[rand.Next(UpperCaseLetters.Length)]);
            password.Append(Digits[rand.Next(Digits.Length)]);
            password.Append(LowerCaseLetters[rand.Next(LowerCaseLetters.Length)]);
            password.Append(LowerCaseLetters[rand.Next(SpecialCharacters.Length)]);

            for (int i = password.Length; i < length; i++)
            {
                password.Append(AllChars[rand.Next(AllChars.Length)]);
            }

            char[] passwordChars = password.ToString().ToCharArray();
            Array.Sort(passwordChars, (x, y) => rand.Next(-1, 2));

            return new string(passwordChars);
        }
    }
}