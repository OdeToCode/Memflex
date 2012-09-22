using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace FlexProviders.Membership
{
    public interface ISecurityEncoder
    {
        string GenerateSalt();
        string Encode(string plainText, string salt);
        string SerializeOAuthProviderUserId(string providerName, string providerUserId);
        bool TryDeserializeOAuthProviderUserID(string protectedData, out string providerName, out string providerUserId);
    }

    public class DefaultSecurityEncoder : ISecurityEncoder
    {
        public string Encode(string plainText, string salt)
        {
            return EncodePassword(plainText, salt);
        }

        public string SerializeOAuthProviderUserId(string providerName, string providerUserId)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(providerName);
                bw.Write(providerUserId);
                bw.Flush();
                var serializedWithPadding = new byte[ms.Length + _padding.Length];
                Buffer.BlockCopy(_padding, 0, serializedWithPadding, 0, _padding.Length);
                Buffer.BlockCopy(ms.GetBuffer(), 0, serializedWithPadding, _padding.Length, (int)ms.Length);
                return MachineKey.Encode(serializedWithPadding, MachineKeyProtection.All);
            }
        }

        public bool TryDeserializeOAuthProviderUserID(string protectedData, out string providerName, out string providerUserId)
        {
            providerName = null;
            providerUserId = null;
            if (String.IsNullOrEmpty(protectedData))
            {
                return false;
            }

            var decodedWithPadding = MachineKey.Decode(protectedData, MachineKeyProtection.All);

            if (decodedWithPadding.Length < _padding.Length)
            {
                return false;
            }

            // timing attacks aren't really applicable to this, so we just do the simple check.
            for (var i = 0; i < _padding.Length; i++)
            {
                if (_padding[i] != decodedWithPadding[i])
                {
                    return false;
                }
            }

            using (var ms = new MemoryStream(decodedWithPadding, _padding.Length, decodedWithPadding.Length - _padding.Length))
            using (var br = new BinaryReader(ms))
            {
                try
                {
                    // use temp variable to keep both out parameters consistent and only set them when the input stream is read completely
                    var name = br.ReadString();
                    var userId = br.ReadString();
                    // make sure that we consume the entire input stream
                    if (ms.ReadByte() == -1)
                    {
                        providerName = name;
                        providerUserId = userId;
                        return true;
                    }
                }
                catch
                {
                    // Any exceptions will result in this method returning false.
                }
            }
            return false;
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

        private static byte[] _padding = new byte[] { 0x85, 0xC5, 0x65, 0x72 };
    }
}
