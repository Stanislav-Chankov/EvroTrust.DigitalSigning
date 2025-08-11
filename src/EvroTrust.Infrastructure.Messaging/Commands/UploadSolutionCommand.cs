using System;

namespace EvroTrust.Infrastructure.Messaging.Commands
{
    public class UploadSolutionCommand
    {
        public Guid CandidateId { get; set; }
        public Guid CodingTaskId { get; set; }
        public Guid CodeSolutionId { get; set; }
        public byte[] EncryptedSolution { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string CandidateEmail { get; set; } = null!;
        public string CandidateFullName { get; set; } = null!;
    }
}