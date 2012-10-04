using Xunit;

namespace FlexProviders.Tests.Integration.Raven.OAuth
{
    public class DeleteAccountTests : IntegrationTest
    {
        [Fact]
        public void Can_Delete_OAuthAccount()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Username = "sallen"});
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", new User { Username = "sallen"});

            var result = MembershipProvider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.True(result);
        }

        [Fact]
        public void Can_Not_Delete_Last_Account()
        {
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", new User { Username = "sallen" });

            var result = MembershipProvider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.False(result);
        }

        [Fact]
        public void Does_Not_Leave_Orphan_Records()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Username = "sallen" });
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", new User { Username = "sallen" });
            MembershipProvider.DissassociateOAuthAccount("Google", "bitmask");

            Assert.DoesNotThrow(() => MembershipProvider.CreateOAuthAccount("Google", "bitmask", new User { Username = "sallen" }));
        }
    }
}