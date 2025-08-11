using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class AssignTaskHandler : IMessageHandler<AssignTaskCommand>
    {
        private readonly ILogger<AssignTaskHandler> _logger;

        public AssignTaskHandler(ILogger<AssignTaskHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(AssignTaskCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled AssignTask for CandidateId: {CandidateId}, TaskTitle: {TaskTitle}", command.CandidateId, command.TaskTitle);
            return Task.CompletedTask;
        }
    }
}