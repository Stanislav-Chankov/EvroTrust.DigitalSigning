namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class CodingTask
    {
        public Guid CodingTaskId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime AssignedAt { get; set; }

        public Guid CandidateId { get; set; }
        public Candidate Candidate { get; set; } = null!;

        public string AssignedBy { get; set; } = string.Empty;

        public ICollection<CodeSolution> CodeSolutions { get; set; } = new List<CodeSolution>();
    }
}