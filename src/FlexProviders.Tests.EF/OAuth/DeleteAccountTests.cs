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
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Username = "sallen" });
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", new User { Username = "sallen" });

            var result = MembershipProvider.DisassociateOAuthAccount("Google", "bitmask");

            Assert.True(result);
        }

        [Fact]
        [AutoRollback]
        public void Can_Not_Delete_Last_Account()
        {
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", new User { Username = "sallen" });

            var result = MembershipProvider.DisassociateOAuthAccount("Google", "bitmask");

            Assert.False(result);
        }

        [Fact]
        [AutoRollback]
        public void Does_Not_Leave_Orphan_Records()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Username = "sallen" });
            MembershipProvider.CreateOAuthAccount("Google", "bitmask", new User { Username = "sallen" });
            MembershipProvider.DisassociateOAuthAccount("Google", "bitmask");

            Assert.DoesNotThrow(() => MembershipProvider.CreateOAuthAccount("Google", "bitmask", new User { Username = "sallen" }));                
        }

    }
}