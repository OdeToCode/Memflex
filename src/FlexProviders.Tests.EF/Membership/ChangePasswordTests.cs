using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.EF.Membership
{
    public class ChangePasswordTests : IntegrationTest
    {
        [Fact]
        [AutoRollback]
        public void Can_Change_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new User { Username = username, Password = password };
            MembershipProvider.CreateAccount(user);

            var firstEncodedPassword = _db.GetPassword(username);
            MembershipProvider.ChangePassword(username, password, "foo");
            var secondEncodedPassword = _db.GetPassword(username);

            Assert.NotEqual(firstEncodedPassword, secondEncodedPassword);
        }
    }
}