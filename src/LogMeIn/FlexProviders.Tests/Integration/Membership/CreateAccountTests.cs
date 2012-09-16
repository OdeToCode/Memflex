using FlexProviders.EF;
using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.Membership
{
    public class CreateAccountTests : IntegrationTest
    {
        [Fact]         
        [AutoRollback]
        public void Can_Create_Account()
        {
            var user = new EfUser() {Username = "sallen", Password = "12345678"};

            _provider.CreateAccount(user);

            Assert.True(_db.CanFindUsername("sallen"));
        }
    }
}