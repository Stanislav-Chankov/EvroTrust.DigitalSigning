using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using EvroTrust.Infrastructure.Messaging.Commands;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class UploadSolutionHandler : IMessageHandler<UploadSolutionCommand>
    {
        private readonly ILogger<UploadSolutionHandler> _logger;
        private readonly IDigitalSigningDbContext _dbContext;

        public UploadSolutionHandler(
            ILogger<UploadSolutionHandler> logger,
            IDigitalSigningDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task HandleAsync(UploadSolutionCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled UploadSolution for CandidateId: {CandidateId}, FileName: {FileName}", command.CandidateId, command.FileName);

            var codeSolution = new CodeSolution
            {
                CodeSolutionId = command.CodeSolutionId,
                CandidateId = command.CandidateId,
                CodingTaskId = command.CodingTaskId,
                EncryptedSolution = command.EncryptedSolution,
                UploadedAt = command.UploadedAt,
                FileName = command.FileName,
                FileType = command.FileType
            };

            await _dbContext.CodeSolutions.AddAsync(codeSolution);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}