using EvroTrust.DigitalSigning.Domain.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace EvroTrust.DigitalSigning.Domain.Services.Infrastructure
{
    public static class ServiceRegistrationExtensions
    {
        // / <summary>
        /// Registers the domain services in the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddDomainServices(this IServiceCollection services)
        {
            // Register all domain services in the assembly by IDomainService
            var domainServiceType = typeof(IDomainService);

            if (domainServiceType.Assembly == null)
            {
                throw new InvalidOperationException("Domain service assembly cannot be null.");
            }

            if (!domainServiceType.IsPublic)
            {
                throw new InvalidOperationException("IDomainService must be a public interface.");
            }

            if (!domainServiceType.IsInterface)
            {
                throw new InvalidOperationException("IDomainService must be an interface.");
            }

            var domainServiceTypes = domainServiceType.Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && domainServiceType.IsAssignableFrom(t));

            foreach (var serviceType in domainServiceTypes)
            {
                services.AddScoped(serviceType);
            }
        }
    }
}
