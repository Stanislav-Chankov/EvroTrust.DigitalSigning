using System;
using System.Collections.Generic;

namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class CodingTask
    {
        public Guid CodingTaskId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime AssignedAt { get; set; }
        public ICollection<CodeSolution> CodeSolutions { get; set; } = new List<CodeSolution>();
    }
}