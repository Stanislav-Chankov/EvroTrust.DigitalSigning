using EvroTrust.DigitalSigning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvroTrust.DigitalSigning.Persistence.Configurations
{
    public class DecisionConfiguration : IEntityTypeConfiguration<Decision>
    {
        public void Configure(EntityTypeBuilder<Decision> builder)
        {
            builder.HasKey(x => x.DecisionId);
            builder.Property(x => x.Reviewer).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
            builder.Property(x => x.Comments).HasMaxLength(1024);
            builder.Property(x => x.DecidedAt).IsRequired();

            builder.HasOne(x => x.CodeSolution)
                   .WithOne(x => x.Decision)
                   .HasForeignKey<Decision>(x => x.CodeSolutionId);

            builder.HasOne(x => x.Candidate)
                   .WithMany(x => x.Decisions)
                   .HasForeignKey(x => x.CandidateId);
        }
    }
}