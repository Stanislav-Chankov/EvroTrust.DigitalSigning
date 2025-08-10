
namespace EvroTrust.Infrastructure.Messaging
{
    public interface IRabbitMqPublisherService
    {
        Task InitializeAsync(RabbitMqOptions options);
        ValueTask PublishAsync(string routingKey, string message);
    }
}