namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public interface IEncryptionService
    {
        byte[] Encrypt(byte[] data);
    }
}