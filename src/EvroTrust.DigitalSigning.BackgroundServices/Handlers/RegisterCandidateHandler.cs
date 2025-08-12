using EvroTrust.DigitalSigning.Extensions;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using EvroTrust.Infrastructure.Messaging.Commands;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class RegisterCandidateHandler : IMessageHandler<RegisterCandidateCommand>
    {
        private readonly IDigitalSigningDbContext _dbContext;
        private readonly ILogger<RegisterCandidateHandler> _logger;

        public RegisterCandidateHandler(
            IDigitalSigningDbContext dbContext,
            ILogger<RegisterCandidateHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task HandleAsync(RegisterCandidateCommand command, CancellationToken cancellationToken = default)
        {
            var candidate = new Candidate
            {
                FullName = command.FullName,
                Email = command.Email,
                Phone = command.Phone,
                DateOfBirth = command.DateOfBirth,
                Address = command.Address,
                City = command.City,
                Country = command.Country,
                LinkedInProfile = command.LinkedInProfile,
                ResumeUrl = command.ResumeUrl
            };

            await _dbContext.Candidates.AddAsync(candidate);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Handled RegisterCandidate: {Candidate}", candidate.ToJsonString());
        }
    }
}