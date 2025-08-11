namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class CodeSolution
    {
        public Guid CodeSolutionId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid CodingTaskId { get; set; }
        public string EncryptedSolution { get; set; }
        public DateTime UploadedAt { get; set; }
        public bool IsReviewed { get; set; }

        public string Reviewer { get; set; } = string.Empty;
        public string? ReviewNotes { get; set; }
        public string? Status { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string CandidateEmail { get; set; } = string.Empty;
        public string CandidateFullName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;

        public Candidate Candidate { get; set; } = null!;
        public CodingTask CodingTask { get; set; } = null!;
        public Decision? Decision { get; set; }
    }
}