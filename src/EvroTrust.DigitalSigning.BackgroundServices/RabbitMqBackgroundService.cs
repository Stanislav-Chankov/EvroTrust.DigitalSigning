using EvroTrust.DigitalSigning.BackgroundServices.Handlers;
using EvroTrust.Infrastructure.Messaging;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EvroTrust.DigitalSigning.BackgroundServices
{
    public class RabbitMqBackgroundService : BackgroundService, IAsyncDisposable
    {
        private readonly ILogger<RabbitMqBackgroundService> _logger;
        private readonly RabbitMqOptions _options;
        private IConnection? _connection;
        private IChannel? _channel;
        private bool _disposed;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqBackgroundService(
            ILogger<RabbitMqBackgroundService> logger,
            IOptions<RabbitMqOptions> options,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        private async Task InitializeAsync()
        {
            var factory = new ConnectionFactory { HostName = _options.HostName };
            SetRabbitMqOptions(_options, factory);

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            const string exchange = "candidate_exchange";
            await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true);

            var queueBindings = new[]
            {
                new { Queue = "candidate_register_queue", RoutingKey = "candidate.register" },
                new { Queue = "task_assign_queue", RoutingKey = "task.assign" },
                new { Queue = "solution_upload_queue", RoutingKey = "solution.upload" },
                new { Queue = "solution_review_queue", RoutingKey = "solution.review" },
                new { Queue = "candidate_decision_queue", RoutingKey = "candidate.decision" }
            };

            foreach (var binding in queueBindings)
            {
                await _channel.QueueDeclareAsync(binding.Queue, durable: true, exclusive: false, autoDelete: false);
                await _channel.QueueBindAsync(binding.Queue, exchange, binding.RoutingKey);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeAsync();

            var queues = new[]
            {
                "candidate_register_queue",
                "task_assign_queue",
                "solution_upload_queue",
                "solution_review_queue",
                "candidate_decision_queue"
            };

            foreach (var queue in queues)
            {
                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation(
                        "Received message from {Queue}: {Message}",
                        queue, message);

                    await HandleMessageAsync(queue, message, stoppingToken);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                };

                await _channel.BasicConsumeAsync(
                    queue: queue,
                    autoAck: false,
                    consumer: consumer);
            }

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleMessageAsync(string queue, string message, CancellationToken cancellationToken)
        {
            try
            {
                switch (queue)
                {
                    case "candidate_register_queue":
                    {
                        var command = JsonSerializer.Deserialize<RegisterCandidateCommand>(message);
                        if (command != null)
                        {
                            var handler = _serviceProvider.GetRequiredService<IMessageHandler<RegisterCandidateCommand>>();
                            await handler.HandleAsync(command, cancellationToken);
                        }
                        break;
                    }
                    case "task_assign_queue":
                    {
                        var command = JsonSerializer.Deserialize<AssignTaskCommand>(message);
                        if (command != null)
                        {
                            var handler = _serviceProvider.GetRequiredService<IMessageHandler<AssignTaskCommand>>();
                            await handler.HandleAsync(command, cancellationToken);
                        }
                        break;
                    }
                    case "solution_upload_queue":
                    {
                        var command = JsonSerializer.Deserialize<UploadSolutionCommand>(message);
                        if (command != null)
                        {
                            var handler = _serviceProvider.GetRequiredService<IMessageHandler<UploadSolutionCommand>>();
                            await handler.HandleAsync(command, cancellationToken);
                        }
                        break;
                    }
                    case "solution_review_queue":
                    {
                        var command = JsonSerializer.Deserialize<ReviewSolutionCommand>(message);
                        if (command != null)
                        {
                            var handler = _serviceProvider.GetRequiredService<IMessageHandler<ReviewSolutionCommand>>();
                            await handler.HandleAsync(command, cancellationToken);
                        }
                        break;
                    }
                    case "candidate_decision_queue":
                    {
                        var command = JsonSerializer.Deserialize<FinalDecisionCommand>(message);
                        if (command != null)
                        {
                            var handler = _serviceProvider.GetRequiredService<IMessageHandler<FinalDecisionCommand>>();
                            await handler.HandleAsync(command, cancellationToken);
                        }
                        break;
                    }
                    default:
                        _logger.LogWarning("Unknown queue: {Queue}", queue);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling message from queue {Queue}: {Message}", queue, message);
            }
        }

        private void SetRabbitMqOptions(RabbitMqOptions options, ConnectionFactory factory)
        {
            if (!string.IsNullOrWhiteSpace(options.UserName))
            {
                factory.UserName = options.UserName;
            }

            if (!string.IsNullOrWhiteSpace(options.Password))
            {
                factory.Password = options.Password;
            }

            if (options.Port.HasValue)
            {
                factory.Port = options.Port.Value;
            }

            if (!string.IsNullOrWhiteSpace(options.VirtualHost))
            {
                factory.VirtualHost = options.VirtualHost;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            if (_channel is IAsyncDisposable asyncChannel)
            {
                await asyncChannel.DisposeAsync();
            }
            else
            {
                _channel?.Dispose();
            }

            if (_connection is IAsyncDisposable asyncConnection)
            {
                await asyncConnection.DisposeAsync();
            }
            else
            {
                _connection?.Dispose();
            }

            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}