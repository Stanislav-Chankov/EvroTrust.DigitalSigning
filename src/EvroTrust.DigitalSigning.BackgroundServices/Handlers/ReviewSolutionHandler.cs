using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class ReviewSolutionHandler : IMessageHandler<ReviewSolutionCommand>
    {
        private readonly ILogger<ReviewSolutionHandler> _logger;

        public ReviewSolutionHandler(ILogger<ReviewSolutionHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(ReviewSolutionCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled ReviewSolution for CandidateId: {CandidateId}, Reviewer: {Reviewer}", command.CandidateId, command.Reviewer);
            return Task.CompletedTask;
        }
    }
}