using System.Web.Mvc;
using FlexProviders.Aspnet;
using FlexProviders.Membership;
using FlexProviders.Roles;
using LogMeIn.Models;
using StructureMap;
namespace LogMeIn {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            x.For<IFilterProvider>().Use<SmFilterProvider>();

                            x.For<IFlexMembershipProvider>().HybridHttpOrThreadLocalScoped().Use<FlexMembershipProvider>();
                            x.For<IFlexRoleProvider>().HybridHttpOrThreadLocalScoped().Use<FlexRoleProvider>();
                            x.For<IFlexUserStore>().HybridHttpOrThreadLocalScoped().Use<UserStore>();
                            x.For<IFlexRoleStore>().HybridHttpOrThreadLocalScoped().Use<RoleStore>();
                            x.SetAllProperties(p => p.OfType<IFlexRoleProvider>());
                            x.Forward<IFlexMembershipProvider, IFlexOAuthProvider>();

                            x.For<IApplicationEnvironment>().Singleton().Use<AspnetEnvironment>();
                            x.For<ISecurityEncoder>().Singleton().Use<DefaultSecurityEncoder>();

                            
                            x.For<MovieDb>().HybridHttpOrThreadLocalScoped().Use<MovieDb>();
                            x.SelectConstructor(() => new MovieDb());                            
                        });
            return ObjectFactory.Container;
        }
    }
}