using System;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Util
{
    public static class Password
    {
        public static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        public static string GenerateSaltedHash(string plainText, string salt)
        {
            return Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(plainText),
                Encoding.UTF8.GetBytes(salt)));
        }

        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            var plainTextWithSaltBytes =
                new byte[plainText.Length + salt.Length];

            for (var i = 0; i < plainText.Length; i++)
                plainTextWithSaltBytes[i] = plainText[i];
            for (var i = 0; i < salt.Length; i++)
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
    }
}