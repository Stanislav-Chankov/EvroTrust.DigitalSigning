namespace EvroTrust.DigitalSigning.BackgroundServices.Abstract
{
    public interface IEncryptionService
    {
        byte[] Encrypt(byte[] data);
    }
}