using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.EF.OAuth
{
    public class DeleteAccountTests : IntegrationTest
    {
        [Fact]
        [AutoRollback]
        public void Can_Delete_OAuthAccount()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", "sallen");

            var result = MembershipProvider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.True(result);
        }

        [Fact]
        [AutoRollback]
        public void Can_Not_Delete_Last_Account()
        {        
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", "sallen");

            var result = MembershipProvider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.False(result);
        }
    }
}