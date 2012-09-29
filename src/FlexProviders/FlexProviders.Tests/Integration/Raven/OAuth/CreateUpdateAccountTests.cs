using System.Linq;
using Xunit;

namespace FlexProviders.Tests.Integration.Raven.OAuth
{
    public class CreateUpdateAccountTests : IntegrationTest
    {
        [Fact]
        public void Can_Create_OAuth_Account()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");

            Assert.Equal(1, Verifier.Query<User>().Single(u=>u.Username == "sallen").OAuthAccounts.Count());
        }

        [Fact]
        public void Can_Update_OAuth_Account()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");
            MembershipProvider.CreateOAuthAccount("Yahoo", "bitmask", "sallen");

            Assert.Equal(2, Verifier.Query<User>().Single(u => u.Username == "sallen").OAuthAccounts.Count());
        }
    }
}