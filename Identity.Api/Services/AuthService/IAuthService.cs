namespace Identity.Api.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> SendConfirmationMail(string email);
        
        Task<bool> ConfirmEmail(string email, string code);

        Task<bool> IsConfirmedEmail(string email);
        
        Task<bool> RegisterByEmail(string email, string password);

        Task<(string, string)> LoginByEmail(string email, string password);

        Task<bool> UserExist(string email);
    }
}
