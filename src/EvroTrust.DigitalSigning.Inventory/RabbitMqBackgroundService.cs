using EvroTrust.Infrastructure.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EvroTrust.DigitalSigning.Inventory
{
    public class RabbitMqBackgroundService : BackgroundService, IAsyncDisposable
    {
        private readonly ILogger<RabbitMqBackgroundService> _logger;
        private readonly RabbitMqOptions _options;
        private IConnection? _connection;
        private IChannel? _channel;
        private bool _disposed;
        //private string _exchangeName = string.Empty;
        //private string _queueName = string.Empty;

        public RabbitMqBackgroundService(
            ILogger<RabbitMqBackgroundService> logger,
            IOptions<RabbitMqOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        private async Task InitializeAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName
            };

            SetRabbitMqOptions(_options, factory);

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            //// 2. Declare exchange (creates if not exists, does nothing if exists)
            //await _channel.ExchangeDeclareAsync(
            //    exchange: _exchangeName,
            //    type: ExchangeType.Direct,
            //    durable: true
            //);

            //// 3. Declare queue
            //await _channel.QueueDeclareAsync(
            //    queue: _queueName,
            //    durable: true,
            //    exclusive: false,
            //    autoDelete: true // optional - deletes when no consumers remain
            //);

            //// 4. Bind queue to exchange
            //await _channel.QueueBindAsync(
            //    queue: _queueName,
            //    exchange: _exchangeName,
            //    routingKey: "tasks"
            //);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeAsync();

            await _channel.QueueDeclareAsync(
                queue: _options.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation(
                    "Received message: {Message}",
                    message);

                await HandleMessageAsync(message, stoppingToken);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(
                queue: _options.QueueName,
                autoAck: false,
                consumer: consumer);

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleMessageAsync(string message, CancellationToken cancellationToken)
        {
            try
            {
                using var doc = JsonDocument.Parse(message);
                if (!doc.RootElement.TryGetProperty("Type", out var typeElement))
                {
                    _logger.LogWarning("Message does not contain a 'Type' property.");
                    return;
                }

                var type = typeElement.GetString();
                switch (type)
                {
                    case "CreateOrder":
                        await HandleCreateOrderAsync(message, cancellationToken);
                        break;
                    case "CancelOrder":
                        await HandleCancelOrderAsync(message, cancellationToken);
                        break;
                    default:
                        _logger.LogWarning("Unknown message type: {Type}", type);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling message: {Message}", message);
            }
        }

        private Task HandleCreateOrderAsync(string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrder: {Message}, {Machine}, {HostName}", message, Environment.MachineName, Environment.GetEnvironmentVariable("HOSTNAME"));
            // TODO: Add your CreateOrder handling logic here
            return Task.CompletedTask;
        }

        private Task HandleCancelOrderAsync(string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CancelOrder: {Message}", message);
            // TODO: Add your CancelOrder handling logic here
            return Task.CompletedTask;
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