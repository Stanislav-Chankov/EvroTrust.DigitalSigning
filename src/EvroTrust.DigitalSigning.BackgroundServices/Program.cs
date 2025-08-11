using EvroTrust.DigitalSigning;
using EvroTrust.DigitalSigning.BackgroundServices;
using EvroTrust.DigitalSigning.BackgroundServices.Handlers;
using EvroTrust.DigitalSigning.Domain.Services;
using EvroTrust.DigitalSigning.Persistence;
using EvroTrust.Infrastructure.Messaging;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

var builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<Worker>();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddHostedService<RabbitMqBackgroundService>();
builder.Services.AddHostedService<DatabaseInitializerService>();

builder.Services.AddScoped<ICandidateService, CandidateService>();

// Register dbContext, repositories, services, etc.
builder.Services.AddDbContext<DigitalSigningDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CandidatesDatabase")));

builder.Services.AddScoped<IMessageHandler<RegisterCandidateCommand>, RegisterCandidateHandler>();
builder.Services.AddScoped<IMessageHandler<AssignTaskCommand>, AssignTaskHandler>();
builder.Services.AddScoped<IMessageHandler<UploadSolutionCommand>, UploadSolutionHandler>();
builder.Services.AddScoped<IMessageHandler<ReviewSolutionCommand>, ReviewSolutionHandler>();

var app = builder.Build();
app.Run();
