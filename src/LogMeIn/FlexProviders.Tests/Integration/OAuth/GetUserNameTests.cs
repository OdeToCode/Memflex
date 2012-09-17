using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.OAuth
{
    public class GetUserNameTests : IntegrationTest
    {
        [Fact] 
        [AutoRollback]
        public void Can_Get_Username_Given_ProviderInfo()
        {
            _provider.CreateOAuthAccount("Microsoft", "bitmask", "sallen");

            var name = _provider.GetUserNameFromOpenAuth("Microsoft", "bitmask");

            Assert.Equal("sallen", name);
        }
    }
}