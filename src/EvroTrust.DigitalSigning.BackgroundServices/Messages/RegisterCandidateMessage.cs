namespace EvroTrust.DigitalSigning.BackgroundServices.Messages
{
    public class RegisterCandidateMessage
    {
        public string Type { get; set; } = "RegisterCandidate";
        public Guid CandidateId { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}