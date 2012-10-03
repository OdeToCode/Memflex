using System;
using System.Configuration;
using System.Web.Mvc;
using FlexProviders.Aspnet;
using FlexProviders.Membership;
using FlexProviders.Mongo;
using FlexProviders.Roles;
using LogMeIn.Models;
using MongoDB.Driver;
using StructureMap;

namespace LogMeIn {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x => {
                    x.Scan(scan => {
                            scan.TheCallingAssembly();
                            scan.WithDefaultConventions();
                        });
                    x.For<IFilterProvider>().Use<SmFilterProvider>();
                    
                    x.For<IFlexMembershipProvider>().HybridHttpOrThreadLocalScoped().Use<FlexMembershipProvider>();
                    x.For<IFlexRoleProvider>().HybridHttpOrThreadLocalScoped().Use<FlexRoleProvider>();
                    x.For<IFlexUserStore>().HybridHttpOrThreadLocalScoped().Use<FlexMembershipUserStore<User, Role>>();
                    x.For<IFlexRoleStore>().HybridHttpOrThreadLocalScoped().Use<FlexMembershipUserStore<User, Role>>();
                    x.SetAllProperties(p => p.OfType<IFlexRoleProvider>());
                    x.Forward<IFlexMembershipProvider, IFlexOAuthProvider>();
                    
                    x.For<IApplicationEnvironment>().Singleton().Use<AspnetEnvironment>();
                    x.For<ISecurityEncoder>().Singleton().Use<DefaultSecurityEncoder>();

                    x.For<MongoDatabase>().Singleton().Use(c => {
                            var connectionString = ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString;
                            if (String.IsNullOrEmpty(connectionString))
                                throw new ConfigurationErrorsException("MongoConnectionString was not found in the App/Web.config.");

                            var url = new MongoUrl(connectionString);
                            var server = MongoServer.Create(url.ToServerSettings());
                            return server.GetDatabase(url.DatabaseName);
                        });
                    x.For<MongoCollection<User>>().Use(c => c.GetInstance<MongoDatabase>().GetCollection<User>("user"));
                    x.For<MongoCollection<Role>>().Use(c => c.GetInstance<MongoDatabase>().GetCollection<Role>("role"));
                });
            return ObjectFactory.Container;
        }
    }
}