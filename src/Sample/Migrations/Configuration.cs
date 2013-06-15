using System.Configuration;
using FlexProviders.Aspnet;
using FlexProviders.Membership;
using FlexProviders.Roles;
using LogMeIn.Models;
using System.Data.Entity.Migrations;

namespace LogMeIn.Migrations
{


    public sealed class Configuration : DbMigrationsConfiguration<LogMeIn.Models.MovieDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Models.MovieDb context)
        {
            var seed = ConfigurationManager.AppSettings["seed"];
            if (seed != "false")
            {

                var membership = new FlexMembershipProvider<User>(new UserStore(context), new AspnetEnvironment());
                var roles = new FlexRoleProvider(new RoleStore(context));

                if (!membership.HasLocalAccount("sallen"))
                {
                    membership.CreateAccount(new User {Username = "sallen", Password = "123", FavoriteNumber = 24});
                }
                if (!roles.RoleExists("admin"))
                {
                    roles.CreateRole("admin");
                }
                if (!roles.IsUserInRole("sallen", "admin"))
                {
                    roles.AddUsersToRoles(new[] {"sallen"}, new[] {"admin"});
                }
            }

            base.Seed(context);
        }
    }
}
