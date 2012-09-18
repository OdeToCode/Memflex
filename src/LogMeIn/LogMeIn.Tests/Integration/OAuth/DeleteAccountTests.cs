using Xunit;
using Xunit.Extensions;

namespace LogMeIn.Tests.Integration.OAuth
{
    public class DeleteAccountTests : IntegrationTest
    {
        [Fact]
        [AutoRollback]
        public void Can_Delete_OAuthAccount()
        {
            _provider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");
            _provider.CreateOAuthAccount("Google", "bitmask", "sallen");

            var result = _provider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.True(result);
        }

        [Fact]
        [AutoRollback]
        public void Can_Not_Delete_Last_Account()
        {        
            _provider.CreateOAuthAccount("Google", "bitmask", "sallen");

            var result = _provider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.False(result);
        }
    }
}