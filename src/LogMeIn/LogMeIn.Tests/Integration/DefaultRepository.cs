using FlexProviders.EF;
using LogMeIn.Models;

namespace LogMeIn.Tests.Integration
{
    public class DefaultUserRepository : FlexMembershipUserRepository<User,MovieDb>
    {
        public DefaultUserRepository() : base(new MovieDb("name=Default"))
        {
            
        }
    }
}