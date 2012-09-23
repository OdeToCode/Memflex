using FlexProviders.EF;

namespace LogMeIn.Models
{
    public class RoleStore : FlexRoleStore<Role,User,MovieDb>
    {
        public RoleStore(MovieDb db) : base(db)
        {
            
        }
    }
}