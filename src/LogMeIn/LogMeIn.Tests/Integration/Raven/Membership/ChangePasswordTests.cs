using System.Linq;
using LogMeIn.Models;
using Xunit;

namespace LogMeIn.Tests.Integration.Raven.Membership
{
    public class ChangePasswordTests : IntegrationTest
    {
        [Fact]
        public void Can_Change_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new User { Username = username, Password = password };
            MembershipProvider.CreateAccount(user);

            var firstEncodedPassword = Verifier.Query<User>().Single(u => u.Username == "sallen").Password;
            MembershipProvider.ChangePassword(username, password, "foo");
            var secondEncodedPassword = Verifier.Query<User>().Single(u => u.Username == "sallen").Password;

            Assert.NotEqual(firstEncodedPassword, secondEncodedPassword);
        }
    }
}