using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FlexProviders.Membership;
using FlexProviders.Roles;
using Microsoft.Web.WebPages.OAuth;
using Raven.Client;

namespace FlexProviders.Raven
{
    public class FlexMembershipUserStore<TUser, TRole> 
        : IFlexUserStore, IFlexRoleStore
          where TUser : class,IFlexMembershipUser, new()
          where TRole : class, IFlexRole<TUser>, new()
    {
        private readonly IDocumentSession _session;

        public FlexMembershipUserStore(IDocumentSession session)
        {
            _session = session;
        }

        public IFlexMembershipUser GetUserByUsername(string username)
        {
            return _session.Query<TUser>().SingleOrDefault(u => u.Username == username);
        }

        public IFlexMembershipUser Add(IFlexMembershipUser user)
        {
            _session.Store(user);
            _session.SaveChanges();
            return user;
        }

        public IFlexMembershipUser Save(IFlexMembershipUser user)
        {
            var existingUser = _session.Query<TUser>().SingleOrDefault(u => u.Username == user.Username);
            foreach(var property in user.GetType().GetProperties().Where(p => p.CanWrite))
            {
                property.SetValue(existingUser, property.GetValue(user));
            }
            _session.SaveChanges();
            return user;
        }

        public bool DeleteOAuthAccount(string provider, string providerUserId)
        {
            var user =
                _session.Query<TUser>()
                        .SingleOrDefault(u => u.OAuthAccounts
                                               .Any(o => o.ProviderUserId == providerUserId && o.Provider == provider));
            
            if(user != null)
            {
                var account =
                    user.OAuthAccounts.Single(o => o.Provider == provider && o.ProviderUserId == providerUserId);
                 user.OAuthAccounts.Remove(account);
                _session.SaveChanges();
                 return true;
            }           
            return false;
        }

        public IFlexMembershipUser GetUserByPasswordResetToken(string passwordResetToken)
        {
            return
                _session.Query<TUser>().SingleOrDefault(u => u.PasswordResetToken == passwordResetToken);
        }

        public IFlexMembershipUser GetUserByOAuthProvider(string provider, string providerUserId)
        {
            return
                _session.Query<TUser>().SingleOrDefault(u => u.OAuthAccounts.Any(r => r.Provider == provider && r.ProviderUserId == providerUserId));
        }

        public IFlexMembershipUser CreateOAuthAccount(string provider, string providerUserId, IFlexMembershipUser user)
        {
            var account = new FlexOAuthAccount {Provider = provider, ProviderUserId = providerUserId};
            if (user.OAuthAccounts == null)
            {
                user.OAuthAccounts = new Collection<FlexOAuthAccount>();
            }
            user.OAuthAccounts.Add(account);           
            _session.SaveChanges();

            return user;
        }

        public IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username)
        {
            return _session
                .Query<TUser>()
                .Single(u => u.Username == username)
                .OAuthAccounts
                .ToArray()
                .Select(o => new OAuthAccount(o.Provider, o.ProviderUserId));
        }

        public void CreateRole(string roleName)
        {
            var role = new TRole {Name = roleName};
            _session.Store(role);
            _session.SaveChanges();
        }

        public string[] GetRolesForUser(string username)
        {
            return _session.Query<TRole>().Where(r => r.Users.Any(name => name == username))
                           .Select(r => r.Name).ToArray();
        }

        public string[] GetUsersInRole(string roleName)
        {
            var role = _session.Query<TRole>().SingleOrDefault(r=> r.Name == roleName);
            if(role != null)
            {
                return role.Users.ToArray();
            }
            return new string[0];
        }

        public string[] GetAllRoles()
        {
            return _session.Query<TRole>().Select(r => r.Name).ToArray();
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                var role = _session.Query<TRole>().Single(r => r.Name == roleName);
                var users = role.Users.Where(usernames.Contains).ToArray();
                foreach(var user in users)
                {
                    role.Users.Remove(user);
                }
            }
            _session.SaveChanges();
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach(var roleName in roleNames)
            {
                var role = _session.Query<TRole>().Single(r => r.Name == roleName);
                foreach(var username in usernames)
                {
                    if(!role.Users.Contains(username))
                    {
                        role.Users.Add(username);
                    }
                }
            }
            _session.SaveChanges();
        }

        public bool RoleExists(string roleName)
        {
            return _session.Query<TRole>().Any(r => r.Name == roleName);
        }

        public bool DeleteRole(string roleName)
        {
            var role = _session.Query<TRole>().SingleOrDefault(r => r.Name == roleName);
            if(role != null)
            {
                _session.Delete(role);
                _session.SaveChanges();
                return true;
            }
            return false;
        }
    }
}