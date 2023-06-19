using System.Net.Mail;
using System.Net;

namespace Identity.Services.EmailService
{
    public class EmailService
    {
        private readonly SmtpClient smtpClient;

        public EmailService(string smtpHost, int smtpPort, string username, string password)
        {
            smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password)
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("youremail@yourdomain.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw;
            }
        }
    }
}
