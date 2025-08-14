namespace EvroTrust.DigitalSigning.WebApi.Models.Requests
{
    public class UploadSolutionRequest
    {
        public Guid CandidateId { get; set; }
        public Guid CodingTaskId { get; set; }
        public Guid CodeSolutionId { get; set; }
        public byte[] EncryptedSolution { get; set; } = Array.Empty<byte>();
        public DateTime UploadedAt { get; set; }
        public string CandidateEmail { get; set; } = string.Empty;
        public string CandidateFullName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
    }
}
