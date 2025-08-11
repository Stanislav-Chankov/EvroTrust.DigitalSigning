using System;

namespace EvroTrust.Infrastructure.Messaging.Commands
{
    public class AssignTaskCommand
    {
        public Guid CandidateId { get; set; }
        public Guid CodingTaskId { get; set; }
        public string TaskTitle { get; set; } = null!;
        public string TaskDescription { get; set; } = null!;
        public DateTime AssignedAt { get; set; }
        public string AssignedBy { get; set; } = null!;
        public string CandidateEmail { get; set; } = null!;
        public string CandidateFullName { get; set; } = null!;
    }
}