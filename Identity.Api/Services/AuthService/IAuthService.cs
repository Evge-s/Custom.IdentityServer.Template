namespace Identity.Api.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> ConfirmEmail(string email, int code);
        
        Task<bool> RegisterByEmail(string email, string password);

        Task<(string, string)> LoginByEmail(string email, string password);
    }
}
