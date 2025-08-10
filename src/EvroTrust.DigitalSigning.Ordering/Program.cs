using EvroTrust.DigitalSigning.Ordering;
using EvroTrust.Infrastructure.Messaging;

var builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<Worker>();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddHostedService<RabbitMqBackgroundService>();

var host = builder.Build();
host.Run();
