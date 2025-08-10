using EvroTrust.DigitalSigning.Ordering;
using EvroTrust.DigitalSigning.Persistence;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddHostedService<RabbitMqBackgroundService>();
builder.Services.AddHostedService<DatabaseInitializerService>();

// Register dbContext, repositories, services, etc.
builder.Services.AddDbContext<DigitalSigningDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderingDatabase")));

builder.Services.AddScoped<IDigitalSigningDbContext>(provider => provider.GetRequiredService<DigitalSigningDbContext>());

var host = builder.Build();
host.Run();
