using EvroTrust.DigitalSigning.BackgroundServices.Abstract;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using EvroTrust.Infrastructure.Messaging.Commands;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class UploadSolutionHandler : IMessageHandler<UploadSolutionCommand>
    {
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<UploadSolutionHandler> _logger;
        private readonly IDigitalSigningDbContext _dbContext;

        public UploadSolutionHandler(
            IEncryptionService encryptionService,
            ILogger<UploadSolutionHandler> logger,
            IDigitalSigningDbContext dbContext)
        {
            _encryptionService = encryptionService;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task HandleAsync(UploadSolutionCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled UploadSolution for CandidateId: {CandidateId}, FileName: {FileName}", command.CandidateId, command.FileName);

            // Encrypt and then base64 encode
            var encryptedBytes = _encryptionService.Encrypt(command.EncryptedSolution);
            var base64EncryptedSolution = Convert.ToBase64String(encryptedBytes);

            var codeSolution = new CodeSolution
            {
                CodeSolutionId = command.CodeSolutionId,
                CandidateId = command.CandidateId,
                CodingTaskId = command.CodingTaskId,
                EncryptedSolution = base64EncryptedSolution,
                UploadedAt = command.UploadedAt,
                FileName = command.FileName,
                FileType = command.FileType
            };

            await _dbContext.CodeSolutions.AddAsync(codeSolution);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}