using FlexProviders.EF;
using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.Membership
{
    public class Login_Tests : IntegrationTest
    {
        [Fact] 
        [AutoRollback]
        public void Can_Login_With_Good_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new EfUser {Username = username, Password = password};
            _provider.CreateAccount(user);

            bool result = _provider.Login(username, password);

            Assert.True(result);
        }

        [Fact]
        [AutoRollback]
        public void Cannot_Login_With_Bad_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new EfUser { Username = username, Password = password };
            _provider.CreateAccount(user);

            bool result = _provider.Login(username, "foo");

            Assert.False(result);
        }
    }
}