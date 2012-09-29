using System.Collections.Generic;
using System.Transactions;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace LogMeIn
{
    public class WebMatrixSecurity : IWebSecurity
    {
        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public void CreateUserAndAccount(string userName, string password)
        {
            WebSecurity.CreateUserAndAccount(userName, password);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public string GetUserName(string provider, string providerUserId)
        {
            return OAuthWebSecurity.GetUserName(provider, providerUserId);
        }

        public ManageMessageId? Dissassociate(string ownerAccount, string provider, string providerUserId)
        {
            // Use a transaction to prevent the user from deleting their last login credential
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
                bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(ownerAccount));
                if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(ownerAccount).Count > 1)
                {
                    OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                    scope.Complete();
                    return ManageMessageId.RemoveLoginSuccess;
                }
            }
            return null;
        }

        public bool HasLocalAccount(string userName)
        {
            return OAuthWebSecurity.HasLocalAccount(GetUserId(userName));
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public bool ChangePassword(string name, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(name, oldPassword, newPassword);
        }

        public void CreateAccount(string name, string newPassword)
        {
            WebSecurity.CreateAccount(name, newPassword);
        }

        public void CreateOrUpdateAccount(string provider, string providerUserId, string name)
        {
            OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, name);
        }

        public string SerializeProviderUserId(string provider, string providerUserId)
        {
            return OAuthWebSecurity.SerializeProviderUserId(provider, providerUserId);
        }

        public AuthenticationClientData GetOAuthClientData(string provider)
        {
            return OAuthWebSecurity.GetOAuthClientData(provider);
        }

        public bool TryDeserializeProviderUserId(string externalLoginData, out string provider, out string providerUserId)
        {
            return OAuthWebSecurity.TryDeserializeProviderUserId(externalLoginData, out provider, out providerUserId);
        }

        public ICollection<AuthenticationClientData> RegisteredClientData
        {
            get { return OAuthWebSecurity.RegisteredClientData; }            
        }

        public ICollection<OAuthAccount> GetAccountsFromUserName(string name)
        {
            return OAuthWebSecurity.GetAccountsFromUserName(name);
        }

        public void RequestAuthentication(string provider, string returnUrl)
        {
            OAuthWebSecurity.RequestAuthentication(provider, returnUrl);
        }

        public AuthenticationResult VerifyAuthentication(string action)
        {
            return OAuthWebSecurity.VerifyAuthentication(action);
        }

        public void Register(string userName, string password)
        {
            CreateUserAndAccount(userName, password);
            Login(userName, password);
        }
    }
}