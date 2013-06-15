using System;
using FlexProviders.Aspnet;
using FlexProviders.Membership;
using FlexProviders.Raven;
using FlexProviders.Roles;
using LogMeIn.Raven.Models;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Document;
namespace Laurelton.Web.Infrastructure.IoC
{
    public class RavenModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentStore>()
                .ToMethod(InitDocStore)
                .InSingletonScope();

            Bind<IDocumentSession>()
                .ToMethod(c => c.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope();
        }

        private IDocumentStore InitDocStore(IContext context)
        {
            IDocumentStore ds =
                new DocumentStore { ConnectionStringName = "RavenDB" }.Initialize();

            CheckForCoreData(ds, context);
            return ds;
        }

        private void CheckForCoreData(IDocumentStore ds, IContext context)
        {
            // In case the versioning bundle is installed, make sure it will version
            // only what we opt-in to version
            using (IDocumentSession s = ds.OpenSession())
            {
                var store = new FlexMembershipUserStore<User, Role>(s);

                var membership = new FlexMembershipProvider<User>(store, new AspnetEnvironment());
                var roles = new FlexRoleProvider(store);
                if (!membership.HasLocalAccount("sallen"))
                {
                    membership.CreateAccount(new User { Username = "sallen", Password = "123", FavoriteNumber = 24 });
                }
                if (!roles.RoleExists("admin"))
                {
                    roles.CreateRole("admin");
                }
                if (!roles.IsUserInRole("sallen", "admin"))
                {
                    roles.AddUsersToRoles(new[] { "sallen" }, new[] { "admin" });
                }


            }
        }
    }
}