using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class FinalDecisionHandler : IMessageHandler<FinalDecisionCommand>
    {
        private readonly ILogger<FinalDecisionHandler> _logger;

        public FinalDecisionHandler(ILogger<FinalDecisionHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(FinalDecisionCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled FinalDecision for CandidateId: {CandidateId}, Status: {Status}", command.CandidateId, command.Status);
            return Task.CompletedTask;
        }
    }
}