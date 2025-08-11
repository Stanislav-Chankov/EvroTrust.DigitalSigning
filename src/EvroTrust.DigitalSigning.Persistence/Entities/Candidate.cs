namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class Candidate
    {
        public Guid CandidateId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string LinkedInProfile { get; set; } = null!;
        public string ResumeUrl { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
        public ICollection<CodeSolution> CodeSolutions { get; set; } = new List<CodeSolution>();
        public ICollection<Decision> Decisions { get; set; } = new List<Decision>();
    }
}