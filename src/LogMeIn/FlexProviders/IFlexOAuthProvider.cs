using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders
{
    public interface IFlexOAuthProvider
    {
        bool Dissassociate(string ownerAccount, string provider, string providerUserId);
        void CreateOrUpdateAccount(string provider, string providerUserId, string username);
        string SerializeProviderUserId(string provider, string providerUserId);
        AuthenticationClientData GetOAuthClientData(string provider);
        bool TryDeserializeProviderUserId(string externalLoginData, out string provider, out string providerUserId);
        ICollection<AuthenticationClientData> RegisteredClientData { get; }
        ICollection<OAuthAccount> GetAccountsFromUserName(string name);
        void RequestAuthentication(string provider, string returnUrl);
        AuthenticationResult VerifyAuthentication(string action); 
    }
}