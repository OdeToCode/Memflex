using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using FlexProviders.Aspnet;
using FlexProviders.Membership;
using FlexProviders.Mongo;
using FlexProviders.Roles;
using LogMeIn.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
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
                    
                    x.For<IFlexMembershipProvider<User>>().HybridHttpOrThreadLocalScoped().Use<FlexMembershipProvider<User>>();
                    x.For<IFlexRoleProvider>().HybridHttpOrThreadLocalScoped().Use<FlexRoleProvider>();
                    x.For<IFlexUserStore<User>>().HybridHttpOrThreadLocalScoped().Use<FlexMembershipUserStore<User, Role>>();
                    x.For<IFlexRoleStore>().HybridHttpOrThreadLocalScoped().Use<FlexMembershipUserStore<User, Role>>();
                    x.SetAllProperties(p => p.OfType<IFlexRoleProvider>());
                    x.Forward<IFlexMembershipProvider<User>, IFlexOAuthProvider<User>>();
                    
                    x.For<IApplicationEnvironment>().Singleton().Use<AspnetEnvironment>();
                    x.For<ISecurityEncoder>().Singleton().Use<DefaultSecurityEncoder>();

                    x.For<MongoDatabase>().Singleton().Use(c => {
                            var connectionString = ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString;
                            if (String.IsNullOrEmpty(connectionString))
                                throw new ConfigurationErrorsException("MongoConnectionString was not found in the App/Web.config.");
                            
                            var url = new MongoUrl(connectionString);
                            var server = MongoServer.Create(url.ToServerSettings());
                            var database = server.GetDatabase(url.DatabaseName);
                            
                            BsonClassMap.RegisterClassMap<User>(ctx => {
                                ctx.SetIgnoreExtraElements(true);
                                ctx.AutoMap();
                                ctx.SetIdMember(ctx.GetMemberMap(u => u.Id).SetRepresentation(BsonType.ObjectId));
                            });
                            
                            BsonClassMap.RegisterClassMap<Role>(ctx => {
                                ctx.SetIgnoreExtraElements(true);
                                ctx.AutoMap();
                                ctx.SetIdMember(ctx.GetMemberMap(r => r.Id).SetRepresentation(BsonType.ObjectId));
                            });
                            
                            var userCollection = database.GetCollection<User>("user");
                            var roleCollection = database.GetCollection<User>("role");
                            
                            userCollection.EnsureIndex(IndexKeys<User>.Ascending(u => u.Username), IndexOptions.SetUnique(true));
                            userCollection.EnsureIndex(IndexKeys.Ascending("OAuthAccounts.Provider", "OAuthAccounts.ProviderUserId"), IndexOptions.SetUnique(true));
                            roleCollection.EnsureIndex(IndexKeys<Role>.Ascending(r => r.Name), IndexOptions.SetUnique(true));
                            roleCollection.EnsureIndex(IndexKeys<Role>.Ascending(r => r.Users));
                            
                            return database;
                        });
                    x.For<MongoCollection<User>>().Use(c => c.GetInstance<MongoDatabase>().GetCollection<User>("user"));
                    x.For<MongoCollection<Role>>().Use(c => c.GetInstance<MongoDatabase>().GetCollection<Role>("role"));
                });
            return ObjectFactory.Container;
        }
    }
}