using EvroTrust.DigitalSigning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvroTrust.DigitalSigning.Persistence.Configurations
{
    public class CodeSolutionConfiguration : IEntityTypeConfiguration<CodeSolution>
    {
        public void Configure(EntityTypeBuilder<CodeSolution> builder)
        {
            builder.HasKey(x => x.CodeSolutionId);
            builder.Property(x => x.EncryptedSolution).IsRequired();
            builder.Property(x => x.UploadedAt).IsRequired();
            builder.Property(x => x.IsReviewed).IsRequired();

            builder.HasOne(x => x.Candidate)
                   .WithMany(x => x.CodeSolutions)
                   .HasForeignKey(x => x.CandidateId);

            builder.HasOne(x => x.CodingTask)
                   .WithMany(x => x.CodeSolutions)
                   .HasForeignKey(x => x.CodingTaskId);

            builder.HasOne(x => x.Decision)
                   .WithOne(x => x.CodeSolution)
                   .HasForeignKey<Decision>(x => x.CodeSolutionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}