namespace Identity.Api.Services.EmailService;

public interface IEmailService
{
    Task<bool> SendMailAsync(string email, string subject, string message);

    string GenerateConfirmationLink(string email, string code);

    string GenerateConfirmationCode();
}