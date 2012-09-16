using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders
{
    public class FlexMemebershipProvider : IFlexMembershipProvider, 
                                           IFlexOAuthProvider,
                                           IOpenAuthDataProvider 
    {
        private readonly IFlexUserRepository _userUserRepository;
        private readonly IFlexOAuthUserRepository _oAuthUserRepository;
        private readonly IApplicationEnvironment _applicationEnvironment;
        private readonly ISecurityEncoder _encoder = new DefaultSecurityEncoder();

        public FlexMemebershipProvider(
            IFlexUserRepository userRepository, 
            IFlexOAuthUserRepository oAuthUserRepository,
            IApplicationEnvironment applicationEnvironment)            
        {         
            _userUserRepository = userRepository;
            _oAuthUserRepository = oAuthUserRepository;
            _applicationEnvironment = applicationEnvironment;
        }

        public bool Login(string username, string password)
        {
            var user = _userUserRepository.GetUserByUsername(username);
            var encodedPassword = _encoder.Encode(password, user.Salt);
            var flag = encodedPassword.Equals(user.Password);
            if(flag)
            {
                _applicationEnvironment.IssueAuthTicket(username, true);
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
            var existingUser = _userUserRepository.GetUserByUsername(user.Username);
            if(existingUser != null)
            {
                throw new MembershipCreateUserException("Cannot register with a duplicate username");
            }

            user.Salt = user.Salt ?? _encoder.GenerateSalt();
            user.Password = _encoder.Encode(user.Password, user.Salt);
            user.IsLocal = true;
            _userUserRepository.Add(user);         
        }

        public bool HasLocalAccount(string userName)
        {
            var user = _userUserRepository.GetUserByUsername(userName);
            return user.IsLocal;
        }

        public string GetUserNameFromOpenAuth(string openAuthProvider, string openAuthId)
        {
            return GetUserName(openAuthProvider, openAuthId);
        }

        public string GetUserName(string provider, string providerUserId)
        {            
            var user = _oAuthUserRepository.GetUserByOAuthProvider(provider, providerUserId);
            return user.Username;
        }

        public bool Dissassociate(string ownerAccount, string provider, string providerUserId)
        {
            var user = _oAuthUserRepository.GetUserByUsername(ownerAccount);
            if(user.IsLocal || user.OAuthAccounts.Count() > 1)
            {
                _oAuthUserRepository.DeleteOAuthAccount(provider, providerUserId);
                return true;
            }
            return false;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _userUserRepository.GetUserByUsername(username);
            var encodedPassword = _encoder.Encode(oldPassword, user.Salt);
            var flag = encodedPassword.Equals(user.Password);
            if(flag)
            {
                user.Password = _encoder.Encode(newPassword, user.Salt);
                _userUserRepository.Save(user);
            }
            return false;
        }

        public void CreateOrUpdateAccount(string provider, string providerUserId, string username)
        {
            _oAuthUserRepository.CreateOrUpdate(provider, providerUserId, username);
        }

        public string SerializeProviderUserId(string providerName, string providerUserId)
        {
            return _encoder.SerializeOAuthProviderUserId(providerName, providerUserId);
        }

        public AuthenticationClientData GetOAuthClientData(string providerName)
        {            
            return _authenticationClients[providerName];
        }

        public void RegisterClient(IAuthenticationClient client, 
            string displayName, IDictionary<string, object> extraData)
        {
            var clientData = new AuthenticationClientData(client, displayName, extraData);
            _authenticationClients.Add(client.ProviderName, clientData);
        }

        public bool TryDeserializeProviderUserId(string protectedData, out string providerName, out string providerUserId)
        {
            return _encoder.TryDeserializeOAuthProviderUserID(protectedData, out providerName, out providerUserId);
        }

        public ICollection<AuthenticationClientData> RegisteredClientData 
        { 
            get { return _authenticationClients.Values; }
        }
        public ICollection<OAuthAccount> GetAccountsFromUserName(string name)
        {
            var user = _oAuthUserRepository.GetUserByUsername(name);
            return user.OAuthAccounts.ToList();
        }

        public void RequestAuthentication(string provider, string returnUrl)
        {
            var client = _authenticationClients[provider];
            _applicationEnvironment.RequestAuthentication(client.AuthenticationClient, this, returnUrl);
        }

        public AuthenticationResult VerifyAuthentication(string returnUrl)
        {
            var providerName = _applicationEnvironment.GetOAuthPoviderName();
            if (String.IsNullOrEmpty(providerName))
            {
                return AuthenticationResult.Failed;
            }

            var client = _authenticationClients[providerName];
            return _applicationEnvironment.VerifyAuthentication(client.AuthenticationClient,this, returnUrl);
        }
        
        private static readonly Dictionary<string, AuthenticationClientData> _authenticationClients =
            new Dictionary<string, AuthenticationClientData>(StringComparer.OrdinalIgnoreCase);        
    }
}