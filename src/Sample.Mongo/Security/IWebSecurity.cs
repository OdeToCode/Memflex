using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace LogMeIn
{

    // TODO: simplify
    public interface IWebSecurity
    {
        bool Login(string userName, string password, bool persistCookie = false);
        void Logout();
        void Register(string userName, string password);        
        bool HasLocalAccount(string userName);
        string GetUserName(string provider, string providerUserId);                
        ManageMessageId? Dissassociate(string ownerAccount, string provider, string providerUserId);                
        bool ChangePassword(string name, string oldPassword, string newPassword);
        void CreateAccount(string name, string newPassword);
        void CreateOrUpdateAccount(string provider, string providerUserId, string name);        
        string SerializeProviderUserId(string provider, string providerUserId);
        AuthenticationClientData GetOAuthClientData(string provider);
        bool TryDeserializeProviderUserId(string externalLoginData, out string provider, out string providerUserId);
        ICollection<AuthenticationClientData> RegisteredClientData { get; }
        ICollection<OAuthAccount> GetAccountsFromUserName(string name);
        void RequestAuthentication(string provider, string returnUrl);
        AuthenticationResult VerifyAuthentication(string action);        
    }
}