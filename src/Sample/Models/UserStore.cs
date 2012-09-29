using FlexProviders.EF;

namespace LogMeIn.Models
{
    public class UserStore : FlexMembershipUserStore<User>
    {
        public UserStore(MovieDb db) : base(db)
        {
            
        }
    }
}