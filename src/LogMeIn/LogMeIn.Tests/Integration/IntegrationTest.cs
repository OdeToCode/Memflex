using FlexProviders.Membership;
using FlexProviders.Roles;
using LogMeIn.Models;

namespace LogMeIn.Tests.Integration
{
    public class IntegrationTest
    {
        protected readonly FlexMembershipProvider MembershipProvider;
        protected readonly FakeApplicationEnvironment Environment;
        protected readonly UserStore UserStore;
        protected readonly RoleStore RoleStore;
        protected readonly FlexRoleProvider RoleProvider;
        
        protected TestDb _db;

        public IntegrationTest()
        {
            var context = new MovieDb("name=Default");
            _db = new TestDb();
            UserStore = new UserStore(context);
            RoleStore = new RoleStore(context);           
            Environment = new FakeApplicationEnvironment();
            RoleProvider = new FlexRoleProvider(RoleStore);
            MembershipProvider = new FlexMembershipProvider(UserStore, Environment);
        }
    }
}