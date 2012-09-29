using System.Linq;
using Xunit;

namespace FlexProviders.Tests.Integration.Raven.Roles
{
    public class CreateRoles : IntegrationTest
    {
        [Fact]
        public void Can_Create_Role()
        {
            RoleProvider.CreateRole("admin");
            Assert.NotNull(Verifier.Query<Role>().SingleOrDefault(r => r.Name == "admin"));
        }

        [Fact]
        public void Can_Add_Users_To_Role()
        {
            var user = new User { Username = "sallen", Password = "123" };
            RoleProvider.CreateRole("admin");
            MembershipProvider.CreateAccount(user);

            RoleProvider.AddUsersToRoles(new[] { "sallen" }, new[] { "admin" });

            Assert.True(Verifier.Query<Role>().SingleOrDefault(r => r.Name == "admin").Users.Contains("sallen"));
        }
    }
}