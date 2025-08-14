using EvroTrust.DigitalSigning.BackgroundServices.Abstract;
using System.Security.Cryptography;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    // <summary>
    // Provides encryption services for sensitive data using AES-256.
    // </summary>
    public class EncryptionService : IEncryptionService
    {
        // In production, store and manage this key securely!
        // This is a valid 32-byte (256-bit) key, Base64-encoded.
        private static readonly byte[] Key = Convert.FromBase64String("uL+6v8Q2v7w9zC2F+F1J9NcRfUjXn2r5u8xA9D2KbPE=");

        public byte[] Encrypt(byte[] data)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Generate a new random IV for each encryption
            aes.GenerateIV();
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor();
            var encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);

            // Prepend IV to the encrypted data for use in decryption
            var result = new byte[iv.Length + encrypted.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

            return result;
        }
    }
}