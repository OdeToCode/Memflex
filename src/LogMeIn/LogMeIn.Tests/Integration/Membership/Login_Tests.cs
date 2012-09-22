using LogMeIn.Models;
using Xunit;
using Xunit.Extensions;

namespace LogMeIn.Tests.Integration.Membership
{
    public class Login_Tests : IntegrationTest
    {
        [Fact] 
        [AutoRollback]
        public void Can_Login_With_Good_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new User {Username = username, Password = password};
            MembershipProvider.CreateAccount(user);

            bool result = MembershipProvider.Login(username, password);

            Assert.True(result);
        }

        [Fact]
        [AutoRollback]
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