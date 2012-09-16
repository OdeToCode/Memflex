using System;
using System.Collections.Generic;
using System.IO;
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

        public FlexMemebershipProvider(
            IFlexUserRepository repository, 
            IFlexOAuthUserRepository oAuthUserRepository)            
        {         
            _userRepository = repository;
            _oAuthUserRepository = oAuthUserRepository;
        }

        public bool VerifyPassword(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            var encodedPassword = _encoder.Encode(password, user.Salt);
            return encodedPassword.Equals(user.Password);
        }

        public void CreateAccount(IFlexMembershipUser user)
        {
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
            var user = _userRepository.GetUserByUsername(userName);
            return user.IsLocal;
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

        public void CreateOrUpdateAccount(string provider, string providerUserId, string username)
        {
            _oAuthUserRepository.CreateOrUpdate(provider, providerUserId, username);
        }

        public string SerializeProviderUserId(string providerName, string providerUserId)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(providerName);
                bw.Write(providerUserId);
                bw.Flush();
                var serializedWithPadding = new byte[ms.Length + _padding.Length];
                Buffer.BlockCopy(_padding, 0, serializedWithPadding, 0, _padding.Length);
                Buffer.BlockCopy(ms.GetBuffer(), 0, serializedWithPadding, _padding.Length, (int) ms.Length);
                return MachineKey.Encode(serializedWithPadding, MachineKeyProtection.All);
            }
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
            providerName = null;
            providerUserId = null;
            if (String.IsNullOrEmpty(protectedData))
            {
                return false;
            }

            var decodedWithPadding = MachineKey.Decode(protectedData, MachineKeyProtection.All);

            if (decodedWithPadding.Length < _padding.Length)
            {
                return false;
            }

            // timing attacks aren't really applicable to this, so we just do the simple check.
            for (var i = 0; i < _padding.Length; i++)
            {
                if (_padding[i] != decodedWithPadding[i])
                {
                    return false;
                }
            }

            using (var ms = new MemoryStream(decodedWithPadding, _padding.Length, decodedWithPadding.Length - _padding.Length))
            using (var br = new BinaryReader(ms))
            {
                try
                {
                    // use temp variable to keep both out parameters consistent and only set them when the input stream is read completely
                    var name = br.ReadString();
                    var userId = br.ReadString();
                    // make sure that we consume the entire input stream
                    if (ms.ReadByte() == -1)
                    {
                        providerName = name;
                        providerUserId = userId;
                        return true;
                    }
                }
                catch
                {
                    // Any exceptions will result in this method returning false.
                }
            }
            return false;

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
            throw new NotImplementedException();
        }

        public AuthenticationResult VerifyAuthentication(string action)
        {
            throw new NotImplementedException();
        }

        private static byte[] _padding = new byte[] { 0x85, 0xC5, 0x65, 0x72 };

        private static readonly Dictionary<string, AuthenticationClientData> _authenticationClients =
            new Dictionary<string, AuthenticationClientData>(StringComparer.OrdinalIgnoreCase);
    }
}