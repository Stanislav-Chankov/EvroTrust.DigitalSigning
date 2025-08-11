using EvroTrust.DigitalSigning.Domain.Services.Abstract;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace EvroTrust.DigitalSigning.Domain.Services
{
    internal class ProductService : IProductService
    {
        private readonly IDigitalSigningDbContext _dbContext;

        public ProductService(IDigitalSigningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            // _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
