using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EvroTrust.Infrastructure.Messaging
{
    /// <summary>
    /// Resource management: The service holds state (connection, channel) that should persist for the application's lifetime, making singleton the appropriate lifetime.
    /// Connection/Channel reuse: RabbitMQ connections and channels are thread-safe for publishing and should be reused rather than created per request. Creating them per request (scoped or transient) would lead to resource exhaustion and degraded performance.
    /// Best practice: Official RabbitMQ guidance recommends sharing a single connection and channel for publishing messages in most application scenarios.
    /// </summary>
    /// <seealso cref="EvroTrust.Infrastructure.Messaging.IRabbitMqPublisherService" />
    /// <seealso cref="System.IAsyncDisposable" />
    public class RabbitMqPublisherService : IRabbitMqPublisherService, IAsyncDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private string _exchangeName = string.Empty;
        private string _queueName = string.Empty;
        private bool _disposed;
        private readonly ILogger<IRabbitMqPublisherService> _logger;
        //private Dictionary<string, string> _featureExchangeMap = new();
        //private Dictionary<string, string> _featureRoutingKeyMap = new();
        //private bool isInitialized = false;

        public RabbitMqPublisherService(ILogger<IRabbitMqPublisherService> logger)
        {
            _logger = logger;
        }

        public async Task InitializeAsync(RabbitMqOptions options)
        {
            _exchangeName = options.ExchangeName;
            _queueName = "inventory_queue"; //options.QueueName;

            var factory = new ConnectionFactory
            {
                HostName = options.HostName
            };

            SetRabbitMqOptions(options, factory);

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            //await _channel.ExchangeDeclareAsync(
            //    exchange: _exchangeName,
            //    type: ExchangeType.Topic,
            //    durable: true
            //);

            //await _channel.QueueDeclareAsync(
            //    queue: _queueName,
            //    durable: true,    // persists messages if broker restarts
            //    exclusive: false, // multiple connections allowed
            //    autoDelete: false // keep queue alive even if no consumers
            //);

            //await _channel.QueueBindAsync(
            //    queue: _queueName,
            //    exchange: "app.direct",
            //    routingKey: "tasks"
            //);
            // 2. Declare exchange (creates if not exists, does nothing if exists)
            await _channel.ExchangeDeclareAsync(
                exchange: _exchangeName,
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

            await _channel.QueueDeclareAsync(
                queue: "inventory_queue",
                durable: true,
                exclusive: false,
                autoDelete: false // optional - deletes when no consumers remain
            );

            // 4. Bind queue to exchange
            await _channel.QueueBindAsync(
                queue: "orders_queue",
                exchange: _exchangeName,
                routingKey: "order"
            );

            await _channel.QueueBindAsync(
                queue: "inventory_queue",
                exchange: _exchangeName,
                routingKey: "inventory"
            );
        }

        public async ValueTask PublishAsync(string routingKey, string message)
        {
            if (!_disposed)
            {
                if (_channel is null)
                {
                    throw new InvalidOperationException("Service not initialized.");
                }

                var body = Encoding.UTF8.GetBytes(message);

                if (string.IsNullOrWhiteSpace(routingKey))
                {
                    throw new ArgumentException("Routing key cannot be null or empty.", nameof(routingKey));
                }

                var properties = new BasicProperties
                {
                    DeliveryMode = DeliveryModes.Persistent,
                    CorrelationId = Guid.NewGuid().ToString(),
                };

                _logger.LogInformation(
                    "Publishing message to RabbitMQ: Exchange={Exchange}, RoutingKey={RoutingKey}, Message={Message}",
                    _exchangeName,
                    routingKey,
                    message);

                await _channel.BasicPublishAsync(
                    exchange: _exchangeName,
                    routingKey: routingKey,
                    mandatory: false,
                    basicProperties: properties,
                    body: body
                );
            }
            else
            {
                throw new ObjectDisposedException(nameof(RabbitMqPublisherService));
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
    }
}
