using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class UploadSolutionHandler : IMessageHandler<UploadSolutionCommand>
    {
        private readonly ILogger<UploadSolutionHandler> _logger;

        public UploadSolutionHandler(ILogger<UploadSolutionHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(UploadSolutionCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled UploadSolution for CandidateId: {CandidateId}, FileName: {FileName}", command.CandidateId, command.FileName);
            return Task.CompletedTask;
        }
    }
}