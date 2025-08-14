namespace EvroTrust.DigitalSigning.BackgroundServices.Abstract
{
    public interface IMessageHandler<TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken = default);
    }
}