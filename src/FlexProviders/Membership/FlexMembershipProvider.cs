using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.Membership
{
    public class FlexMembershipProvider : IFlexMembershipProvider, 
                                           IFlexOAuthProvider,
                                           IOpenAuthDataProvider 
    {
        private readonly IFlexUserStore _userStore;
        private readonly IApplicationEnvironment _applicationEnvironment;
        private readonly ISecurityEncoder _encoder = new DefaultSecurityEncoder();

        public FlexMembershipProvider(
            IFlexUserStore userStore, 
            IApplicationEnvironment applicationEnvironment)            
        {         
            _userStore = userStore;
            _applicationEnvironment = applicationEnvironment;
        }

        public bool Login(string username, string password, bool rememberMe = false)
        {
            var user = _userStore.GetUserByUsername(username);
            if(user == null)
            {
                return false;
            }

            var encodedPassword = _encoder.Encode(password, user.Salt);
            var passed = encodedPassword.Equals(user.Password);
            if (passed)
            {
                _applicationEnvironment.IssueAuthTicket(username, rememberMe);
                return true;
            }
            return false;
        }

        public void Logout()
        {
            _applicationEnvironment.RevokeAuthTicket();
        }

        public void CreateAccount(IFlexMembershipUser user)
        {
            var existingUser = _userStore.GetUserByUsername(user.Username);
            if (existingUser != null)
            {
                throw new MembershipCreateUserException("Cannot register with a duplicate username");
            }

            user.Salt = user.Salt ?? _encoder.GenerateSalt();
            user.Password = _encoder.Encode(user.Password, user.Salt);
            user.IsLocal = true;
            _userStore.Add(user);
        }

        public bool HasLocalAccount(string userName)
        {
            var user = _userStore.GetUserByUsername(userName);
            return user != null && user.IsLocal;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _userStore.GetUserByUsername(username);
            var encodedPassword = _encoder.Encode(oldPassword, user.Salt);
            var flag = encodedPassword.Equals(user.Password);
            if (flag)
            {
                user.Password = _encoder.Encode(newPassword, user.Salt);
                _userStore.Save(user);
            }
            return false;
        }

        public void CreateOAuthAccount(string provider, string providerUserId, IFlexMembershipUser user)
        {
            var existingUser = _userStore.GetUserByUsername(user.Username);
            if(existingUser == null)
            {
                user.IsLocal = false;
                _userStore.Add(user);
            }
            _userStore.CreateOAuthAccount(provider, providerUserId, existingUser ?? user);
        }

        public string GetUserNameFromOpenAuth(string provider, string providerUserId)
        {
            var user = _userStore.GetUserByOAuthProvider(provider, providerUserId);
            if(user != null)
            {
                return user.Username;
            }
            return String.Empty;
        }

        public bool DissassociateOAuthAccount(string provider, string providerUserId)
        {
            var user = _userStore.GetUserByOAuthProvider(provider, providerUserId);
            if(user == null)
            {
                return false;
            }
            if(user.IsLocal)
            {
                return _userStore.DeleteOAuthAccount(provider, providerUserId);
            }
            var accounts = _userStore.GetOAuthAccountsForUser(user.Username);
            if(accounts.Count() > 1)
            {
                return _userStore.DeleteOAuthAccount(provider, providerUserId);
            }
            return false;
        }

        public AuthenticationClientData GetOAuthClientData(string providerName)
        {
            return _authenticationClients[providerName];
        }

        public ICollection<AuthenticationClientData> RegisteredClientData
        {
            get { return _authenticationClients.Values; }
        }

        public IEnumerable<OAuthAccount> GetOAuthAccountsFromUserName(string username)
        {
            return _userStore.GetOAuthAccountsForUser(username);
        }

        public void RequestOAuthAuthentication(string provider, string returnUrl)
        {
            var client = _authenticationClients[provider];
            _applicationEnvironment.RequestAuthentication(client.AuthenticationClient, this, returnUrl);
        }

        public AuthenticationResult VerifyOAuthAuthentication(string returnUrl)
        {
            var providerName = _applicationEnvironment.GetOAuthPoviderName();
            if (String.IsNullOrEmpty(providerName))
            {
                return AuthenticationResult.Failed;
            }

            var client = _authenticationClients[providerName];
            return _applicationEnvironment.VerifyAuthentication(client.AuthenticationClient, this, returnUrl);
        }

        public bool OAuthLogin(string provider, string providerUserId, bool persistCookie)
        {
            var oauthProvider = _authenticationClients[provider];
            var context = _applicationEnvironment.AcquireContext();
            var securityManager = new OpenAuthSecurityManager(context, oauthProvider.AuthenticationClient, this);
            return securityManager.Login(providerUserId, persistCookie);
        }

        public static void RegisterClient(IAuthenticationClient client,
           string displayName, IDictionary<string, object> extraData)
        {
            var clientData = new AuthenticationClientData(client, displayName, extraData);
            _authenticationClients.Add(client.ProviderName, clientData);
        }

        private static readonly Dictionary<string, AuthenticationClientData> _authenticationClients =
            new Dictionary<string, AuthenticationClientData>(StringComparer.OrdinalIgnoreCase);        
    }
}