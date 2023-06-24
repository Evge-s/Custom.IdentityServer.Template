namespace Identity.Api.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> SendConfirmationMail(string email);
        
        Task<bool> ConfirmEmail(string email, string code);

        Task<bool> RegisterByEmail(string email, string password);

        Task<(string, string)> LoginByEmail(string email, string password);

        Task RemoveAccount(string accId);
        
        Task BlockAccount(string accId);

        Task RevokeRefreshToken(string accId, string refreshToken);

        Task ChangePassword(string accId, string oldPassword, string newPassword);

        Task ResetPassword(string email, string resetToken, string newPassword);

        Task<bool> UserExist(string email);
    }
}
