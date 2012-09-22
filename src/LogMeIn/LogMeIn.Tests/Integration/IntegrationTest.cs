using FlexProviders;
using FlexProviders.EF;
using FlexProviders.Membership;
using FlexProviders.Roles;
using LogMeIn.Models;

namespace LogMeIn.Tests.Integration
{
    public class IntegrationTest
    {
        protected readonly FlexMemebershipProvider MembershipProvider;
        protected readonly FakeApplicationEnvironment Environment;
        protected readonly DefaultUserStore UserStore;
        protected readonly FlexRoleProvider RoleProvider;
        
        protected TestDb _db;

        public IntegrationTest()
        {
            var context = new MovieDb("name=Default");
            _db = new TestDb();
            UserStore = new DefaultUserStore(context);
            RoleProvider = new FlexRoleProvider();
            RoleProvider.Initialize(new DefaultRoleStore(context));
            Environment = new FakeApplicationEnvironment();
            MembershipProvider = new FlexMemebershipProvider(UserStore,UserStore, Environment);
        }
    }
}