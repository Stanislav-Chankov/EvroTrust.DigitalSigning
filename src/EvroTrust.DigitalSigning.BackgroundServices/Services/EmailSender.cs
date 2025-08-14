using EvroTrust.DigitalSigning.BackgroundServices.Abstract;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EvroTrust.DigitalSigning.BackgroundServices.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                _logger.LogWarning("Email address is null or empty.");
                return;
            }

            // TODO: Move these settings to a configuration file or environment variables
            var smtpServer = "smtp.yourserver.com"; // Replace with your SMTP server
            var username = "yourusername"; // Replace with your SMTP username
            var password = "yourpassword"; // Replace with your SMTP password
            var sendFrom = "support@evrotrust.com";

            // Configure your SMTP settings here
            using var smtpClient = new SmtpClient(smtpServer)
            {
                Port = 587,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(sendFrom),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent to {Email}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
            }
        }
    }
}