using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.EF.OAuth
{
    public class GetUserNameTests : IntegrationTest
    {
        [Fact] 
        [AutoRollback]
        public void Can_Get_Username_Given_ProviderInfo()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Username = "sallen" });

            var name = MembershipProvider.GetUserNameFromOpenAuth("Microsoft", "bitmask");

            Assert.Equal("sallen", name);
        }
    }
}