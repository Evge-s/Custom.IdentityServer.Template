using System.Net;
using System.Net.Mail;
using System.Web;

namespace Identity.Api.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string serviceUserName;

        public EmailService(string domain, int port, string username, string password)
        {
            serviceUserName = username;
            
            _smtpClient = new SmtpClient()
            {
                Host = domain, 
                Port = port, 
                Credentials = new NetworkCredential(username, password), 
                EnableSsl = true
            };
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage(serviceUserName, email, subject, message);
            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
        }

        public string GenerateConfirmationLink(string email)
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            var encodedCode = HttpUtility.UrlEncode(code);
            var link = $"https://yourdomain.com/confirm?email={email}&code={encodedCode}";
            return link;
        }
    }
}
