using EvroTrust.DigitalSigning.BackgroundServices.Abstract;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EvroTrust.DigitalSigning.BackgroundServices.Services
{
    /// <summary>
    /// Provides email sending functionality with advanced retry logic for transient failures.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        // Polly retry policy for handling transient failures with backoff
        private readonly AsyncRetryPolicy _retryPolicy;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
            // Configure Polly to retry up to 5 times with backoff for SmtpException and IOException
            _retryPolicy = Policy
                .Handle<SmtpException>()
                .Or<System.IO.IOException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    // Use backoff for more robust retry intervals
                    sleepDurationProvider: attempt =>
                    {
                        // Base delay: 1 second, max delay: 30 seconds
                        var baseDelay = TimeSpan.FromSeconds(1);
                        var maxDelay = TimeSpan.FromSeconds(10);
  
                        var jitter = new Random().NextDouble() + 0.5;
                        var delay = TimeSpan.FromSeconds(Math.Min(baseDelay.TotalSeconds * Math.Pow(2, attempt) * jitter, maxDelay.TotalSeconds));
                        return delay;
                    },
                    onRetry: (exception, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception, "Retry {RetryCount} after {Delay}s due to transient failure sending email.", retryCount, timespan.TotalSeconds);
                    });
        }

        /// <inheritdoc/>
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

            // Use Polly retry policy to handle transient failures when sending email
            await _retryPolicy.ExecuteAsync(async () =>
            {
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
                    // Rethrow to allow Polly to handle retries
                    throw;
                }
            });
        }
    }
}