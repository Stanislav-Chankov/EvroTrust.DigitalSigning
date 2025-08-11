using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EvroTrust.DigitalSigning.Persistence
{
    public static class DbUpdateExceptionExtensions
    {
        public static Exception GetDetailedDbUpdateException(this DbUpdateException ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("A database update error occurred.");

            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception:");
                sb.AppendLine(ex.InnerException.Message);
            }

            if (ex.Entries != null && ex.Entries.Any())
            {
                sb.AppendLine("Entities involved:");
                foreach (var entry in ex.Entries)
                {
                    sb.AppendLine($"- Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
                }
            }

            // Use InvalidOperationException as a more specific exception type
            return new InvalidOperationException(sb.ToString(), ex);
        }
    }
}