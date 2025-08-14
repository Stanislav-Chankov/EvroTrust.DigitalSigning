using EvroTrust.DigitalSigning.BackgroundServices.Abstract;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.EntityFrameworkCore;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class ReviewSolutionHandler : IMessageHandler<ReviewSolutionCommand>
    {
        private readonly IDigitalSigningDbContext _dbContext;
        private readonly ILogger<ReviewSolutionHandler> _logger;

        public ReviewSolutionHandler(
            IDigitalSigningDbContext digitalSigningDbContext,
            ILogger<ReviewSolutionHandler> logger)
        {
            _dbContext = digitalSigningDbContext;
            _logger = logger;
        }

        public async Task HandleAsync(ReviewSolutionCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled ReviewSolution for CandidateId: {CandidateId}, Reviewer: {Reviewer}", command.CandidateId, command.Reviewer);

            // Load and update the entity
            var codeSolution = await _dbContext.CodeSolutions.FirstOrDefaultAsync(cs => cs.CodeSolutionId == command.CodeSolutionId, cancellationToken)
                               ?? throw new InvalidOperationException("Code solution not found.");

            codeSolution.Reviewer = command.Reviewer;
            codeSolution.ReviewedAt = command.ReviewedAt ?? System.DateTime.UtcNow;
            codeSolution.Status = command.Status;
            codeSolution.IsReviewed = command.IsReviewed;

            // Entity is already tracked and updated in handler
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}