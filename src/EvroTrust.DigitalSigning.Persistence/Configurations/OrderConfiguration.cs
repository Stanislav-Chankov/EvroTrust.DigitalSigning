using EvroTrust.DigitalSigning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvroTrust.DigitalSigning.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.OrderId);
            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(o => o.OrderDate)
                .IsRequired();
            builder.HasMany(o => o.Products)
                .WithMany(p => p.Orders);

            //builder.HasOne(o => o.Payment)
            //    .WithOne(p => p.Order)
            //    .HasForeignKey<Payment>(p => p.Order);

            // Order can have multiple shipments, but each shipment belongs to one order
            builder.HasMany(o => o.Shipments)
                .WithOne(s => s.Order)
                .HasForeignKey(s => s.OrderId);
        }
    }
}
