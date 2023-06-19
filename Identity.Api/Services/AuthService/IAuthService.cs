namespace Identity.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> Register(string email, string password);

        Task<(string, string)> Login(string email, string password);
    }
}
