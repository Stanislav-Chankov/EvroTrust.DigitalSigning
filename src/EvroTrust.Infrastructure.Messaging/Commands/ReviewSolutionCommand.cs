namespace EvroTrust.Infrastructure.Messaging.Commands;

public class ReviewSolutionCommand
{
    public Guid CodeSolutionId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid CodingTaskId { get; set; }
    public string Reviewer { get; set; } = null!;
    public bool IsReviewed { get; set; }
    public string? ReviewComments { get; set; }
    public string? Status { get; set; } // e.g. "Accepted", "Rejected"
    public DateTime? ReviewedAt { get; set; }
    public string CandidateEmail { get; set; } = null!;
    public string CandidateFullName { get; set; } = null!;
}