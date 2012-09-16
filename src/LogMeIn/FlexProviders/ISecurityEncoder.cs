using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace FlexProviders
{
    public interface ISecurityEncoder
    {
        string GenerateSalt();
        string Encode(string plainText, string salt);
    }

    public class DefaultSecurityEncoder : ISecurityEncoder
    {
        public string Encode(string plainText, string salt)
        {
            Contract.Requires(plainText != null);
            Contract.Requires(salt != null);            
            return EncodePassword(plainText, salt);
        }

        public string GenerateSalt()
        {            
            var buffer = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        private string EncodePassword(string password, string salt)
        {            
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);            

            var hashStrategy = HashAlgorithm.Create("HMACSHA256") as KeyedHashAlgorithm;
            if (hashStrategy.Key.Length == saltBytes.Length)
            {
                hashStrategy.Key = saltBytes;
            }
            else if (hashStrategy.Key.Length < saltBytes.Length)
            {
                var keyBytes = new byte[hashStrategy.Key.Length];
                Buffer.BlockCopy(saltBytes, 0, keyBytes, 0, keyBytes.Length);
                hashStrategy.Key = keyBytes;
            }
            else
            {
                var keyBytes = new byte[hashStrategy.Key.Length];
                for (var i = 0; i < keyBytes.Length; )
                {
                    var len = Math.Min(saltBytes.Length, keyBytes.Length - i);
                    Buffer.BlockCopy(saltBytes, 0, keyBytes, i, len);
                    i += len;
                }
                hashStrategy.Key = keyBytes;
            }
            var result = hashStrategy.ComputeHash(passwordBytes);
            return Convert.ToBase64String(result);
        }
    }
}
