using EvroTrust.DigitalSigning.Persistence;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using EvroTrust.Infrastructure.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EvroTrust.DigitalSigning.Ordering
{
    public class RabbitMqBackgroundService : BackgroundService, IAsyncDisposable
    {
        private readonly ILogger<RabbitMqBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqOptions _options;
        private IConnection? _connection;
        private IChannel? _channel;
        private bool _disposed;

        public RabbitMqBackgroundService(
            ILogger<RabbitMqBackgroundService> logger,
            IOptions<RabbitMqOptions> options,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
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

            // 2. Declare exchange (creates if not exists, does nothing if exists)
            await _channel.ExchangeDeclareAsync(
                exchange: "my_topic_exchange",
                type: ExchangeType.Direct,
                durable: true
            );

            // 3. Declare queue
            await _channel.QueueDeclareAsync(
                queue: "orders_queue",
                durable: true,
                exclusive: false,
                autoDelete: false // optional - deletes when no consumers remain
            );

            // 4. Bind queue to exchange
            await _channel.QueueBindAsync(
                queue: "orders_queue",
                exchange: "my_topic_exchange",
                routingKey: "order"
            );
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<IDigitalSigningDbContext>();

                    _logger.LogInformation("Adding Product to the database.......");

                    //await dbContext.Products.AddAsync(new Product
                    //{
                    //    Name = "Test Product",
                    //    Price = 100.00m,
                    //    Quantity = 10,
                    //});
                    //await dbContext.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation("Added Product to the database!!!!!!!!!!!!!!!!!!");
                }

                _logger.LogInformation("Starting RabbitMQ background service...");

                await InitializeAsync();

                //await _channel.QueueDeclareAsync(
                //    queue: "orders_queue",
                //    durable: true,
                //    exclusive: false,
                //    autoDelete: false,
                //    arguments: null);

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

                _logger.LogInformation("Consume from queue", _options.QueueName);

                await _channel.BasicConsumeAsync(
                    queue: _options.QueueName,
                    autoAck: false,
                    consumer: consumer);

                _logger.LogInformation("Ordering successfully running");

                // Keep the service running
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
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