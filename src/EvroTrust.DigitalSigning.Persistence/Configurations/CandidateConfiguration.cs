using EvroTrust.DigitalSigning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvroTrust.DigitalSigning.Persistence.Configurations
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.HasKey(x => x.CandidateId);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(32);
            builder.Property(x => x.RegisteredAt).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasMany(x => x.CodeSolutions)
                   .WithOne(x => x.Candidate)
                   .HasForeignKey(x => x.CandidateId);

            builder.HasMany(x => x.Decisions)
                   .WithOne(x => x.Candidate)
                   .HasForeignKey(x => x.CandidateId);
        }
    }
}