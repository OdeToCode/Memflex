using System.Web.Security;
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

        [Fact]
        [AutoRollback]
        public void Fails_If_Duplicate_Username()
        {
            var user = new EfUser() { Username = "sallen", Password = "12345678" };

            _provider.CreateAccount(user);

            Assert.Throws<MembershipCreateUserException>(() => _provider.CreateAccount(user));
        }

        [Fact]
        [AutoRollback]
        public void Account_Created_As_Local_Account()
        {
            var user = new EfUser() { Username = "sallen", Password = "12345678" };
            
            _provider.CreateAccount(user);
            
            Assert.True(_provider.HasLocalAccount("sallen"));
        }
    }
}