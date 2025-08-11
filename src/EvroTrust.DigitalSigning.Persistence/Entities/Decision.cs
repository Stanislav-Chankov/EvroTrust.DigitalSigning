using System;

namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class Decision
    {
        public Guid DecisionId { get; set; }
        public Guid CodeSolutionId { get; set; }
        public Guid CandidateId { get; set; }
        public string Reviewer { get; set; } = null!;
        public string Status { get; set; } = null!; // e.g. "Accepted", "Rejected"
        public string? Comments { get; set; }
        public DateTime DecidedAt { get; set; }
        public CodeSolution CodeSolution { get; set; } = null!;
        public Candidate Candidate { get; set; } = null!;
    }
}