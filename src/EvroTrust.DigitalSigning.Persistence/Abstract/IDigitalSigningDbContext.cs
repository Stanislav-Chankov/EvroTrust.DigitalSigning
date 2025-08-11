using EvroTrust.DigitalSigning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EvroTrust.DigitalSigning.Persistence.Abstract
{
    public interface IDigitalSigningDbContext
    {
        //DbSet<Order> Orders { get; set; }
        //DbSet<Payment> Payments { get; set; }
        //DbSet<Shipment> Shipments { get; set; }
        //DbSet<Product> Products { get; set; }

        DbSet<Candidate> Candidates { get; set; }
        DbSet<CodeSolution> CodeSolutions { get; set; }
        DbSet<CodingTask> CodingTasks { get; set; }
        DbSet<Decision> Decisions { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}