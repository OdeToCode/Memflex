using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders
{
    public interface IFlexOAuthProvider
    {
        bool DissassociateOAuthAccount(string provider, string providerUserId);
        void CreateOAuthAccount(string provider, string providerUserId, string username);        
        AuthenticationClientData GetOAuthClientData(string provider);        
        ICollection<AuthenticationClientData> RegisteredClientData { get; }
        IEnumerable<OAuthAccount> GetOAuthAccountsFromUserName(string username);
        void RequestOAuthAuthentication(string provider, string returnUrl);
        AuthenticationResult VerifyOAuthAuthentication(string action);
        bool OAuthLogin(string provider, string providerUserId, bool persistCookie);
    }
}