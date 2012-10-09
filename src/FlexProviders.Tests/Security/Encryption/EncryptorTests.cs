using System;
using FlexProviders.Membership;
using Xunit;

namespace FlexProviders.Tests.Security.Encryption
{
    public class EncryptorTests
    {
        [Fact] 
        public void Can_Hash_A_Password()
        {
            var encryptor = new DefaultSecurityEncoder();
            string salt = encryptor.GenerateSalt();
            var result1 = encryptor.Encode("plainText", salt);
            var result2 = encryptor.Encode("plainText", salt);

            Assert.False(String.IsNullOrEmpty(result1));
            Assert.NotEqual("plainText", result1);
            Assert.Equal(result1, result2);
        }
    }
}