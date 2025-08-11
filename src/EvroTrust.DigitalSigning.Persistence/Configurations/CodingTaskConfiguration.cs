using EvroTrust.DigitalSigning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvroTrust.DigitalSigning.Persistence.Configurations
{
    public class CodingTaskConfiguration : IEntityTypeConfiguration<CodingTask>
    {
        public void Configure(EntityTypeBuilder<CodingTask> builder)
        {
            builder.HasKey(x => x.CodingTaskId);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(2048);
            builder.Property(x => x.AssignedAt).IsRequired();

            builder.HasMany(x => x.CodeSolutions)
                   .WithOne(x => x.CodingTask)
                   .HasForeignKey(x => x.CodingTaskId);
        }
    }
}