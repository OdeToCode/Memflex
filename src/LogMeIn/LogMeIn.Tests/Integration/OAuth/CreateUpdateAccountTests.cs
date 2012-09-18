using Xunit;
using Xunit.Extensions;

namespace LogMeIn.Tests.Integration.OAuth
{
    public class CreateUpdateAccountTests : IntegrationTest
    {
        [Fact]
        [AutoRollback]
        public void Can_Create_OAuth_Account()
        {
            _provider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");

            Assert.True(_db.GetCountOfOAuthAccounts("sallen") == 1);
        }

        [Fact]
        [AutoRollback]
        public void Can_Update_OAuth_Account()
        {
            _provider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");
            _provider.CreateOAuthAccount("Yahoo", "bitmask", "sallen");

            Assert.True(_db.GetCountOfOAuthAccounts("sallen") == 2);
        }
    }
}