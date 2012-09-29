using Xunit;

namespace FlexProviders.Tests.Integration.Raven.OAuth
{
    public class GetUserNameTests : IntegrationTest
    {
        [Fact]
        public void Can_Get_Username_Given_ProviderInfo()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");

            var name = MembershipProvider.GetUserNameFromOpenAuth("Microsoft", "bitmask");

            Assert.Equal("sallen", name);
        }
    }
}