using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.EF.OAuth
{
    public class CreateUpdateAccountTests : IntegrationTest
    {
        [Fact]
        [AutoRollback]
        public void Can_Create_OAuth_Account()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Username = "sallen" });

            Assert.Equal(1, _db.GetCountOfOAuthAccounts("sallen"));
        }

        [Fact]
        [AutoRollback]
        public void Can_Update_OAuth_Account()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Username = "sallen" });
            MembershipProvider.CreateOAuthAccount("Yahoo", "bitmask", new User { Username = "sallen" });

            Assert.Equal(2, _db.GetCountOfOAuthAccounts("sallen"));
        }
    }
}