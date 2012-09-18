using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders
{
    public interface IFlexOAuthUserRepository
    {
        bool DeleteOAuthAccount(string provider, string providerUserId);        
        IFlexMembershipUser GetUserByOAuthProvider(string provider, string providerUserId);        
        IFlexMembershipUser CreateOAuthAccount(string provider, string providerUserId, string username);
        IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username);
    }
}