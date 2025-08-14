namespace EvroTrust.DigitalSigning.WebApi.Models.Requests
{
    public class AddFinalDecisionRequest
    {
        public Guid DecisionId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid CodeSolutionId { get; set; }
        public string Reviewer { get; set; } = null!;
        public string Status { get; set; } = null!; // e.g. "Accepted", "Rejected"
        public string? Comments { get; set; }
        public DateTime DecidedAt { get; set; }
        public string CandidateEmail { get; set; } = null!;
        public string CandidateFullName { get; set; } = null!;
    }
}
