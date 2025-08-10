using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvroTrust.DigitalSigning.Persistence
{
    public class DigitalSigningDbContext : DbContext, IDigitalSigningDbContext
    {
        public DigitalSigningDbContext(DbContextOptions<DigitalSigningDbContext> options)
         : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<Payment> Payments { get; set; } = default!;
        public DbSet<Shipment> Shipments { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;


        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw ex.GetDetailedDbUpdateException();
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw ex.GetDetailedDbUpdateException();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Register entity configurations
            builder.ApplyConfigurationsFromAssembly(typeof(DigitalSigningDbContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
