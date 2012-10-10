using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.Membership
{
    public class FlexMembershipProvider : IFlexMembershipProvider, 
                                           IFlexOAuthProvider,
                                           IOpenAuthDataProvider 
    {
        private const int TokenSizeInBytes = 16;
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
                throw new FlexMembershipException(FlexMembershipStatus.DuplicateUserName);
            }

            user.Salt = user.Salt ?? _encoder.GenerateSalt();
            user.Password = _encoder.Encode(user.Password, user.Salt);
            _userStore.Add(user);
        }

        public void UpdateAccount(IFlexMembershipUser user)
        {
            _userStore.Save(user);
        }

        public bool HasLocalAccount(string userName)
        {
            var user = _userStore.GetUserByUsername(userName);
            return user != null && !String.IsNullOrEmpty(user.Password);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _userStore.GetUserByUsername(username);
            var encodedPassword = _encoder.Encode(oldPassword, user.Salt);
            if (!encodedPassword.Equals(user.Password))
                return false;

            user.Password = _encoder.Encode(newPassword, user.Salt);
            _userStore.Save(user);
            return true;
        }

        public void SetLocalPassword(string username, string newPassword)
        {
            var user = _userStore.GetUserByUsername(username);
            if (!String.IsNullOrEmpty(user.Password))
            {
                throw new FlexMembershipException("SetLocalPassword can only be used on accounts that currently don't have a local password.");
            }

            user.Salt = _encoder.GenerateSalt();
            user.Password = _encoder.Encode(newPassword, user.Salt);
            _userStore.Save(user);
        }

        public string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow = 1440)
        {
            var user = _userStore.GetUserByUsername(username);
            if (user == null)
            {
                throw new FlexMembershipException(FlexMembershipStatus.InvalidUserName);
            }

            user.PasswordResetToken = GenerateToken();
            user.PasswordResetTokenExpiration = DateTime.Now.AddMinutes(tokenExpirationInMinutesFromNow);
            _userStore.Save(user);

            return user.PasswordResetToken;
        }

        public bool ResetPassword(string passwordResetToken, string newPassword)
        {
            var user = _userStore.GetUserByPasswordResetToken(passwordResetToken);
            if (user == null)
                return false;

            if (String.IsNullOrEmpty(user.Salt))
                user.Salt = _encoder.GenerateSalt();
            user.Password = _encoder.Encode(newPassword, user.Salt);
            _userStore.Save(user);

            return true;
        }

        private static string GenerateToken() {
            using (var prng = new RNGCryptoServiceProvider()) {
                return GenerateToken(prng);
            }
        }

        internal static string GenerateToken(RandomNumberGenerator generator) {
            byte[] tokenBytes = new byte[TokenSizeInBytes];
            generator.GetBytes(tokenBytes);
            return HttpServerUtility.UrlTokenEncode(tokenBytes);
        }

        public void CreateOAuthAccount(string provider, string providerUserId, IFlexMembershipUser user)
        {
            var existingUser = _userStore.GetUserByUsername(user.Username);
            if(existingUser == null)
            {
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