using FlexProviders.EF;
using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.Membership
{
    public class ChangePasswordTests : IntegrationTest
    {
        [Fact]
        [AutoRollback]
        public void Can_Change_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new EfUser { Username = username, Password = password };
            _provider.CreateAccount(user);

            var firstEncodedPassword = _db.GetPassword(username);
            _provider.ChangePassword(username, password, "foo");
            var secondEncodedPassword = _db.GetPassword(username);

            Assert.NotEqual(firstEncodedPassword, secondEncodedPassword);
        }
    }
}