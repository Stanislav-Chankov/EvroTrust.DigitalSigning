using EvroTrust.DigitalSigning.Domain.Services;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.EntityFrameworkCore;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class AssignTaskHandler : IMessageHandler<AssignTaskCommand>
    {
        private readonly ILogger<AssignTaskHandler> _logger;
        private readonly IDigitalSigningDbContext _dbContext;

        public AssignTaskHandler(
            ILogger<AssignTaskHandler> logger,
            IDigitalSigningDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task HandleAsync(AssignTaskCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled AssignTask for CandidateId: {CandidateId}, TaskTitle: {TaskTitle}", command.CandidateId, command.TaskTitle);

            var candidate = await _dbContext.Candidates.FirstOrDefaultAsync(c => c.CandidateId == command.CandidateId, cancellationToken)
                ?? throw new InvalidOperationException("Candidate not found.");

            var codingTask = new CodingTask
            {
                CodingTaskId = command.CodingTaskId,
                Title = command.TaskTitle,
                Description = command.TaskDescription,
                AssignedAt = command.AssignedAt,
                AssignedBy = command.AssignedBy,
                CandidateId = command.CandidateId
            };

            _dbContext.CodingTasks.Add(codingTask);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}