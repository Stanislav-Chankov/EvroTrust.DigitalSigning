using EvroTrust.DigitalSigning.Ordering;

var builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<RabbitMqBackgroundService>();

var host = builder.Build();
host.Run();
