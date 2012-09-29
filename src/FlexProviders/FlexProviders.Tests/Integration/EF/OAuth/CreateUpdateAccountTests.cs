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
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");

            Assert.True(_db.GetCountOfOAuthAccounts("sallen") == 1);
        }

        [Fact]
        [AutoRollback]
        public void Can_Update_OAuth_Account()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");
            MembershipProvider.CreateOAuthAccount("Yahoo", "bitmask", "sallen");

            Assert.True(_db.GetCountOfOAuthAccounts("sallen") == 2);
        }
    }
}