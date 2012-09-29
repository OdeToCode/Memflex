using Xunit;

namespace FlexProviders.Tests.Integration.Raven.OAuth
{
    public class DeleteAccountTests : IntegrationTest
    {
        [Fact]
        public void Can_Delete_OAuthAccount()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", "sallen");

            var result = MembershipProvider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.True(result);
        }

        [Fact]
        public void Can_Not_Delete_Last_Account()
        {
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", "sallen");

            var result = MembershipProvider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.False(result);
        }
    }
}