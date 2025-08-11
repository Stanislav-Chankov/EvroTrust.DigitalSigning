using System;

namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class CodeSolution
    {
        public Guid CodeSolutionId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid CodingTaskId { get; set; }
        public byte[] EncryptedSolution { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
        public bool IsReviewed { get; set; }
        public Candidate Candidate { get; set; } = null!;
        public CodingTask CodingTask { get; set; } = null!;
        public Decision? Decision { get; set; }
    }
}