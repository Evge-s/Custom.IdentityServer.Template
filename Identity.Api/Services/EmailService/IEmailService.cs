namespace Identity.Api.Services.EmailService;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string email, string subject, string message);
}