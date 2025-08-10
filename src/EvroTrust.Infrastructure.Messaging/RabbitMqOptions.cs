namespace EvroTrust.Infrastructure.Messaging
{
    public class RabbitMqOptions
    {
        public required string HostName { get; set; }
        public required string ExchangeName { get; set; }
        public required string QueueName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? Port { get; set; }
        public string? VirtualHost { get; set; }
    }

    public class RabbitMqOptions2
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; }
        public List<RabbitMqFeature> Features { get; set; } = new();
    }

    public class RabbitMqFeature
    {
        public string Name { get; set; } = string.Empty;
        public RabbitMqExchange Exchange { get; set; } = new();
        public RabbitMqQueue Queue { get; set; } = new();
        public List<RabbitMqBinding> Bindings { get; set; } = new();
    }

    public class RabbitMqExchange
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "direct"; // direct, topic, fanout, headers
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
        public Dictionary<string, object>? Arguments { get; set; }
    }

    public class RabbitMqQueue
    {
        public string Name { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public Dictionary<string, object>? Arguments { get; set; }
    }

    public class RabbitMqBinding
    {
        public string Queue { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
    }
}