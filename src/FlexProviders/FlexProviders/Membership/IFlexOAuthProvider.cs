using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.Membership
{
    public interface IFlexOAuthProvider
    {
        bool OAuthLogin(string provider, string providerUserId, bool persistCookie);
        void RequestOAuthAuthentication(string provider, string returnUrl);
        AuthenticationResult VerifyOAuthAuthentication(string action);
        void CreateOAuthAccount(string provider, string providerUserId, string username);
        bool DissassociateOAuthAccount(string provider, string providerUserId);
        AuthenticationClientData GetOAuthClientData(string provider);        
        ICollection<AuthenticationClientData> RegisteredClientData { get; }
        IEnumerable<OAuthAccount> GetOAuthAccountsFromUserName(string username);        
    }
}