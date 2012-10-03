using FlexProviders.EF;

namespace FlexProviders.Tests.Integration.EF
{
    public class UserStore : FlexMembershipUserStore<User>
    {
        public UserStore(SomeDb db) : base(db)
        {
            
        }
    }
}