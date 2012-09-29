using System.Linq;
using System.Threading;
using System.Web.Security;
using LogMeIn.Models;
using Xunit;
using Xunit.Extensions;

namespace LogMeIn.Tests.Integration.Raven.Membership
{
    public class CreateAccountTests : IntegrationTest
    {
        [Fact]         
        public void Can_Create_Account()
        {
            var user = new User() {Username = "sallen", Password = "12345678"};

            MembershipProvider.CreateAccount(user);
          
            user = Verifier.Query<User>().SingleOrDefault(u => u.Username == "sallen");

            Assert.NotNull(user);
        }

        [Fact]
        public void Fails_If_Duplicate_Username()
        {
            var user1 = new User() { Username = "sallen", Password = "12345678" };
            var user2 = new User() {Username = "sallen", Password = "4567890"};

            MembershipProvider.CreateAccount(user1);
             
            Assert.Throws<MembershipCreateUserException>(() => MembershipProvider.CreateAccount(user2));
        }

        [Fact]
        public void Account_Created_As_Local_Account()
        {
            var user = new User() { Username = "sallen", Password = "12345678" };
            
            MembershipProvider.CreateAccount(user);
            
            Assert.True(MembershipProvider.HasLocalAccount("sallen"));
        }
    }
}