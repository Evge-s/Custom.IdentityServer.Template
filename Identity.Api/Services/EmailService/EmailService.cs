using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Identity.Api.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly string _serviceUserName;

        public EmailService(string apiKey, string serviceUserName)
        {
            _serviceUserName = serviceUserName;
            _client = new SendGridClient(apiKey);
        }

        public async Task<bool> SendMailAsync(string email, string subject, string message)
        {
            var from = new EmailAddress(_serviceUserName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            try
            {
                var response = await _client.SendEmailAsync(msg);
                return response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                // Log
                throw e;
            }
        }

        public string GenerateConfirmationLink(string email, string code)
        {
            var link = $"https://yourdomain.com/confirm?email={email}&code={code}";
            return link;
        }

        public string GenerateConfirmationCode()
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            return WebUtility.UrlEncode(code);
        }
    }
}