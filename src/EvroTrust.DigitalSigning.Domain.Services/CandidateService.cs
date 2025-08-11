using System;
using System.Threading;
using System.Threading.Tasks;
using EvroTrust.DigitalSigning.Persistence;
using EvroTrust.DigitalSigning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvroTrust.DigitalSigning.Domain.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly DigitalSigningDbContext _dbContext;

        public CandidateService(DigitalSigningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Candidate> RegisterCandidateAsync(Candidate candidate, CancellationToken cancellationToken = default)
        {
            candidate.CandidateId = Guid.NewGuid();
            candidate.RegisteredAt = DateTime.UtcNow;

            _dbContext.Candidates.Add(candidate);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return candidate;
        }

        public async Task<Candidate?> GetCandidateByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Candidates.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }
    }
}