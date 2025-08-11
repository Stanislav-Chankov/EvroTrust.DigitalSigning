using System.Threading;
using System.Threading.Tasks;
using EvroTrust.DigitalSigning.Persistence.Entities;

namespace EvroTrust.DigitalSigning.Domain.Services
{
    public interface ICandidateService
    {
        Task<Candidate> RegisterCandidateAsync(Candidate candidate, CancellationToken cancellationToken = default);
        Task<Candidate?> GetCandidateByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}