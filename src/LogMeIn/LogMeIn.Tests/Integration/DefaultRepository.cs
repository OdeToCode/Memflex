using FlexProviders.EF;
using LogMeIn.Models;

namespace LogMeIn.Tests.Integration
{
    public class DefaultUserStore : FlexMembershipUserStore<User,MovieDb>
    {
        public DefaultUserStore() : base(new MovieDb("name=Default"))
        {
            
        }
    }
}