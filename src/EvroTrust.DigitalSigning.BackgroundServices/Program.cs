using EvroTrust.DigitalSigning.BackgroundServices;
using EvroTrust.DigitalSigning.BackgroundServices.Abstract;
using EvroTrust.DigitalSigning.BackgroundServices.Handlers;
using EvroTrust.DigitalSigning.BackgroundServices.Services;
using EvroTrust.DigitalSigning.Persistence;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.Infrastructure.Messaging;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddHostedService<RabbitMqBackgroundService>();


// Register dbContext, repositories, services, etc.
builder.Services.AddDbContext<IDigitalSigningDbContext, DigitalSigningDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CandidatesDatabase")));

builder.Services.AddScoped<IMessageHandler<AssignTaskCommand>, AssignTaskHandler>();
builder.Services.AddScoped<IMessageHandler<RegisterCandidateCommand>, RegisterCandidateHandler>();
builder.Services.AddScoped<IMessageHandler<UploadSolutionCommand>, UploadSolutionHandler>();
builder.Services.AddScoped<IMessageHandler<ReviewSolutionCommand>, ReviewSolutionHandler>();
builder.Services.AddScoped<IMessageHandler<FinalDecisionCommand>, FinalDecisionHandler>();

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();
app.Run();
