using EvroTrust.DigitalSigning.Persistence.Entities;

namespace EvroTrust.DigitalSigning.Domain.Services.Abstract
{
    public interface IProductService
    {
        Task AddProductAsync(Product product, CancellationToken cancellationToken = default);
    }
}
