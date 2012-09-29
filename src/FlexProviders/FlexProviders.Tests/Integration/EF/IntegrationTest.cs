using FlexProviders.Membership;
using FlexProviders.Roles;

namespace FlexProviders.Tests.Integration.EF
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
            var context = new SomeDb("name=Default");
            _db = new TestDb();
            UserStore = new UserStore(context);
            RoleStore = new RoleStore(context);           
            Environment = new FakeApplicationEnvironment();
            RoleProvider = new FlexRoleProvider(RoleStore);
            MembershipProvider = new FlexMembershipProvider(UserStore, Environment);
        }
    }
}