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

            var result = encryptor.Encode("plainText", "11111111");

            Assert.False(String.IsNullOrEmpty(result));
            Assert.NotEqual("plainText", result);
        }
    }
}