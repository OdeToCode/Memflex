using System.Configuration.Provider;
using System.Linq;
using Xunit;

namespace FlexProviders.Tests.Integration.Raven.Roles
{
    public class DeleteRoles : IntegrationTest
    {
        [Fact]
        public void Can_Delete_Role()
        {
            RoleProvider.CreateRole("admin");

            RoleProvider.DeleteRole("admin", true);

            Assert.False(Verifier.Query<Role>().Any(r => r.Name == "admin"));
        }

        [Fact]
        public void Can_Delete_Role_With_Users()
        {
            var user = new User { Username = "sallen", Password = "123" };
            RoleProvider.CreateRole("admin");
            MembershipProvider.CreateAccount(user);
            RoleProvider.AddUsersToRoles(new[] { "sallen" }, new[] { "admin" });

            RoleProvider.DeleteRole("admin", false);

            Assert.False(Verifier.Query<Role>().Any(r => r.Name == "admin"));
        }

        [Fact]
        public void Can_Throw_When_Users_In_Role()
        {
            var user = new User { Username = "sallen", Password = "123" };
            RoleProvider.CreateRole("admin");
            MembershipProvider.CreateAccount(user);
            RoleProvider.AddUsersToRoles(new[] { "sallen" }, new[] { "admin" });

            Assert.Throws<ProviderException>(() => RoleProvider.DeleteRole("admin", true));
        }

        [Fact]
        public void Can_Remove_Users_From_Roles()
        {
            var user1 = new User { Username = "sallen", Password = "123" };
            var user2 = new User { Username = "missmm", Password = "123" };

            RoleProvider.CreateRole("admin");
            RoleProvider.CreateRole("engineering");
            RoleProvider.CreateRole("sales");

            MembershipProvider.CreateAccount(user1);
            MembershipProvider.CreateAccount(user2);

            RoleProvider.AddUsersToRoles(new[] { "sallen", "missmm" }, new[] { "admin", "engineering" });

            RoleProvider.RemoveUsersFromRoles(new[] { "sallen", "missmm" }, new[] { "sales", "admin" });

            Assert.True(RoleProvider.IsUserInRole("sallen", "engineering"));
            Assert.False(RoleProvider.IsUserInRole("sallen", "admin"));
        }
    }
}