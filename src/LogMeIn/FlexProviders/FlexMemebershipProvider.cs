using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders
{
    public class FlexMemebershipProvider : IFlexMembershipProvider
    {
        private readonly IFlexUserRepository _userRepository;
        private readonly IFlexOAuthUserRepository _oAuthUserRepository;
        private readonly ISecurityEncoder _encoder = new DefaultSecurityEncoder();

        public FlexMemebershipProvider(IFlexUserRepository repository, IFlexOAuthUserRepository oAuthUserRepository)            
        {
            Contract.Requires(repository != null);
           
            _userRepository = repository;
            _oAuthUserRepository = oAuthUserRepository;
        }

        public bool VerifyPassword(string username, string password)
        {
            Contract.Requires(username != null);
            Contract.Requires(password != null);

            var user = _userRepository.GetUserByUsername(username);
            var encodedPassword = _encoder.Encode(password, user.Salt);
            return encodedPassword.Equals(user.Password);
        }

        public void CreateAccount(IFlexMembershipUser user)
        {
            Contract.Requires(user != null);      
            Contract.Requires(user.Password != null);
            Contract.Requires(user.Username != null);

            var existingUser = _userRepository.GetUserByUsername(user.Username);
            if(existingUser != null)
            {
                throw new MembershipCreateUserException("Cannot register with a duplicate username");
            }

            user.Salt = user.Salt ?? _encoder.GenerateSalt();
            user.Password = _encoder.Encode(user.Password, user.Salt);
            user.IsLocal = true;
            _userRepository.Add(user);         
        }

        public bool HasLocalAccount(string userName)
        {
            Contract.Requires(userName != null);

            var user = _userRepository.GetUserByUsername(userName);
            return user.IsLocal;
        }

        public string GetUserName(string provider, string providerUserId)
        {
            Contract.Requires(provider !=null);
            Contract.Requires(providerUserId != null);

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
            Contract.Requires(username != null);
            Contract.Requires(oldPassword != null);
            Contract.Requires(newPassword != null);

            var user = _userRepository.GetUserByUsername(username);
            var encodedPassword = _encoder.Encode(oldPassword, user.Salt);
            var flag = encodedPassword.Equals(user.Password);
            if(flag)
            {
                user.Password = _encoder.Encode(newPassword, user.Salt);
                _userRepository.Save(user);
            }
            return false;
        }

        public void CreateOrUpdateAccount(string provider, string providerUserId, string name)
        {
            throw new NotImplementedException();
        }

        public string SerializeProviderUserId(string provider, string providerUserId)
        {
            throw new NotImplementedException();
        }

        public AuthenticationClientData GetOAuthClientData(string provider)
        {
            throw new NotImplementedException();
        }

        public bool TryDeserializeProviderUserId(string externalLoginData, out string provider, out string providerUserId)
        {
            throw new NotImplementedException();
        }

        public ICollection<AuthenticationClientData> RegisteredClientData { get; private set; }
        public ICollection<OAuthAccount> GetAccountsFromUserName(string name)
        {
            throw new NotImplementedException();
        }

        public void RequestAuthentication(string provider, string returnUrl)
        {
            throw new NotImplementedException();
        }

        public AuthenticationResult VerifyAuthentication(string action)
        {
            throw new NotImplementedException();
        }
    }
}

