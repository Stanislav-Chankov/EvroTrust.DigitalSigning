using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.EntityFrameworkCore;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class FinalDecisionHandler : IMessageHandler<FinalDecisionCommand>
    {
        private readonly ILogger<FinalDecisionHandler> _logger;
        private readonly IDigitalSigningDbContext _dbContext;

        public FinalDecisionHandler(
            ILogger<FinalDecisionHandler> logger,
            IDigitalSigningDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task HandleAsync(FinalDecisionCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled FinalDecision for CandidateId: {CandidateId}, Decision: {Decision}", command.CandidateId, command.DecisionId);

            var candidate = await _dbContext.Candidates.FirstOrDefaultAsync(c => c.CandidateId == command.CandidateId, cancellationToken);
            if (candidate == null)
                throw new InvalidOperationException("Candidate not found.");

            var finalDecision = new Decision
            {
                CandidateId = command.CandidateId,
                Status = command.Status,
                DecidedAt = command.DecidedAt,
                Reviewer = command.Reviewer,
                Comments = command.Comments
            };

            _dbContext.Decisions.Add(finalDecision);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}