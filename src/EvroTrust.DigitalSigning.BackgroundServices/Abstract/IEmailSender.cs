namespace EvroTrust.DigitalSigning.BackgroundServices.Abstract
{
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}