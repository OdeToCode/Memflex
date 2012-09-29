using FlexProviders.EF;

namespace LogMeIn.Models
{
    public class RoleStore : FlexRoleStore<Role,User>
    {
        public RoleStore(MovieDb db) : base(db)
        {
            
        }
    }
}