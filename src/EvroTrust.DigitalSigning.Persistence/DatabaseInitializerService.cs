using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EvroTrust.DigitalSigning.Persistence
{
    public class DatabaseInitializerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseInitializerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DigitalSigningDbContext>();

            // Ensure the database is created
            await dbContext.Database.MigrateAsync(stoppingToken);
            await dbContext.Database.EnsureCreatedAsync(stoppingToken);
        }
    }
}