using FlexProviders.EF;

namespace FlexProviders.Tests.Integration.EF
{
    public class RoleStore : FlexRoleStore<Role,User>
    {
        public RoleStore(SomeDb db) : base(db)
        {
            
        }
    }
}