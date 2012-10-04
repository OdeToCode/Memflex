using System.Collections.Generic;

namespace FlexProviders.Membership
{
    public interface IFlexMembershipUser
    {
        string Username { get; set; }
        string Password { get; set; }
        string Salt { get; set; }
        bool IsLocal { get; set; }
        ICollection<FlexOAuthAccount> OAuthAccounts { get; set; }       
    }
}