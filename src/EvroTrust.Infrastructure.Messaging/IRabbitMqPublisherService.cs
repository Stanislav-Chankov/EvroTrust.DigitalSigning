
namespace EvroTrust.Infrastructure.Messaging
{
    public interface IRabbitMqPublisherService
    {
        Task InitializeAsync(RabbitMqOptions options);
        ValueTask PublishAsync<TMessage>(string routingKey, TMessage message);
    }
}