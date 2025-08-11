namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public interface IMessageHandler<TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken = default);
    }
}