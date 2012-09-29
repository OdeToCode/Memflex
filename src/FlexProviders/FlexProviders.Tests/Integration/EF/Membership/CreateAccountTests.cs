using System.Web.Security;
using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.EF.Membership
{
    public class CreateAccountTests : IntegrationTest
    {
        [Fact]         
        [AutoRollback]
        public void Can_Create_Account()
        {
            var user = new User() {Username = "sallen", Password = "12345678"};

            MembershipProvider.CreateAccount(user);

            Assert.True(_db.CanFindUsername("sallen"));
        }

        [Fact]
        [AutoRollback]
        public void Fails_If_Duplicate_Username()
        {
            var user = new User() { Username = "sallen", Password = "12345678" };

            MembershipProvider.CreateAccount(user);

            Assert.Throws<MembershipCreateUserException>(() => MembershipProvider.CreateAccount(user));
        }

        [Fact]
        [AutoRollback]
        public void Account_Created_As_Local_Account()
        {
            var user = new User() { Username = "sallen", Password = "12345678" };
            
            MembershipProvider.CreateAccount(user);
            
            Assert.True(MembershipProvider.HasLocalAccount("sallen"));
        }
    }
}