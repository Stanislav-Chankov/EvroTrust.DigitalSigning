namespace EvroTrust.DigitalSigning.WebApi.Models.Requests
{
    public class RegisterCandidateRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string LinkedInProfile { get; set; } = null!;
        public string ResumeUrl { get; set; } = null!;
    }
}
