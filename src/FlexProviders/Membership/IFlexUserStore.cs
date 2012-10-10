using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.Membership
{
    public interface IFlexUserStore
    {        
        IFlexMembershipUser Add(IFlexMembershipUser user);        
        IFlexMembershipUser Save(IFlexMembershipUser user);
        IFlexMembershipUser CreateOAuthAccount(string provider, string providerUserId, IFlexMembershipUser user);        
        IFlexMembershipUser GetUserByUsername(string username);
        IFlexMembershipUser GetUserByOAuthProvider(string provider, string providerUserId);        
        IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username);
        bool DeleteOAuthAccount(string provider, string providerUserId);
        IFlexMembershipUser GetUserByPasswordResetToken(string passwordResetToken);
    }    
}