using System.Threading;
using System.Threading.Tasks;
using EvroTrust.DigitalSigning.Domain.Services;
using EvroTrust.DigitalSigning.Extensions;
using EvroTrust.DigitalSigning.Persistence.Entities;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class RegisterCandidateHandler : IMessageHandler<RegisterCandidateCommand>
    {
        private readonly ICandidateService _candidateService;
        private readonly ILogger<RegisterCandidateHandler> _logger;

        public RegisterCandidateHandler(ICandidateService candidateService, ILogger<RegisterCandidateHandler> logger)
        {
            _candidateService = candidateService;
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

            await _candidateService.RegisterCandidateAsync(candidate, cancellationToken);

            _logger.LogInformation("Handled RegisterCandidate: {Candidate}", candidate.ToJsonString());
        }
    }
}