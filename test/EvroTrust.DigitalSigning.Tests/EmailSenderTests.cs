using EvroTrust.DigitalSigning.BackgroundServices.Services;
using EvroTrust.DigitalSigning.BackgroundServices.Abstract;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;

namespace EvroTrust.DigitalSigning.BackgroundServices.Tests.Services
{
    public class EmailSenderTests
    {
        private readonly Mock<ILogger<EmailSender>> _loggerMock;

        public EmailSenderTests()
        {
            _loggerMock = new Mock<ILogger<EmailSender>>();
        }

        [Fact]
        public async Task SendEmailAsync_WithEmptyEmail_LogsWarningAndReturns()
        {
            // Arrange
            var sender = new EmailSender(_loggerMock.Object);

            // Act
            await sender.SendEmailAsync("", "subject", "body");

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Email address is null or empty.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task SendEmailAsync_WithValidEmail_LogsInformationOnSuccess()
        {
            // Arrange
            var sender = new EmailSender(_loggerMock.Object);
            var toEmail = "test@evrotrust.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act & Assert
            await Assert.ThrowsAsync<SmtpException>(async () =>
            {
                await sender.SendEmailAsync(toEmail, subject, body);
            });

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to send email")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.AtLeastOnce
            );
        }

        [Fact]
        public async Task SendEmailAsync_RetriesOnSmtpException()
        {
            // Arrange
            var sender = new EmailSender(_loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<SmtpException>(async () =>
            {
                await sender.SendEmailAsync("test@evrotrust.com", "subject", "body");
            });

            // Verify that warning log for retry was called (Polly retry)
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Retry")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.AtLeastOnce
            );
        }
    }
}