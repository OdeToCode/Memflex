using Xunit;

namespace FlexProviders.Tests.Integration.Raven.Membership
{
    public class LoginTests : IntegrationTest
    {
        [Fact]
        public void Can_Login_With_Good_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new User { Username = username, Password = password };
            MembershipProvider.CreateAccount(user);

            bool result = MembershipProvider.Login(username, password);

            Assert.True(result);
        }

        [Fact]
        public void Cannot_Login_With_Bad_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new User { Username = username, Password = password };
            MembershipProvider.CreateAccount(user);

            bool result = MembershipProvider.Login(username, "foo");

            Assert.False(result);
        }
    }
}