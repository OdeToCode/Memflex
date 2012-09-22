using FlexProviders.EF;
using LogMeIn.Models;

namespace LogMeIn.Tests.Integration
{
    public class DefaultUserStore : FlexMembershipUserStore<User,MovieDb>                                  
    {
        public DefaultUserStore(MovieDb context) : base(context) 
        {
            
        }
    }

    public class DefaultRoleStore : FlexRoleStore<Role, User, MovieDb>
    {
        public DefaultRoleStore(MovieDb context) : base(context)
        {
            
        }
    }
}