using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using FlexProviders.Membership;
using FlexProviders.Roles;
using Microsoft.Web.WebPages.OAuth;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace FlexProviders.Mongo
{
    public class FlexMembershipUserStore<TUser, TRole> : IFlexUserStore, IFlexRoleStore
        where TUser : class, IFlexMembershipUser, new()
        where TRole : class, IFlexRole, new()
    {
        private readonly MongoCollection<TRole> _roleCollection;
        private readonly MongoCollection<TUser> _userCollection;

        public FlexMembershipUserStore(MongoCollection<TUser> userCollection, MongoCollection<TRole> roleCollection)
        {
            _userCollection = userCollection;
            _roleCollection = roleCollection;
        }

        public void CreateRole(string roleName)
        {
            var role = new TRole {Name = roleName};
            _roleCollection.Save(role);
        }

        public string[] GetRolesForUser(string username)
        {
            return _roleCollection.AsQueryable().Where(r => r.Users.Any(name => name == username))
                .Select(r => r.Name).ToArray();
        }

        public string[] GetUsersInRole(string roleName)
        {
            TRole role = _roleCollection.AsQueryable().SingleOrDefault(r => r.Name == roleName);
            if (role != null)
                return role.Users.ToArray();

            return new string[0];
        }

        public string[] GetAllRoles()
        {
            return _roleCollection.AsQueryable().Select(r => r.Name).ToArray();
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (string roleName in roleNames)
            {
                TRole role = _roleCollection.AsQueryable().Single(r => r.Name == roleName);
                string[] users = role.Users.Where(usernames.Contains).ToArray();
                foreach (string user in users)
                    role.Users.Remove(user);
                _roleCollection.Save(role);
            }
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string roleName in roleNames)
            {
                TRole role = _roleCollection.AsQueryable().Single(r => r.Name == roleName);
                foreach (string username in usernames)
                {
                    if (!role.Users.Contains(username))
                    {
                        role.Users.Add(username);
                    }
                }
                _roleCollection.Save(role);
            }
        }

        public bool RoleExists(string roleName)
        {
            return _roleCollection.AsQueryable().Any(r => r.Name == roleName);
        }

        public bool DeleteRole(string roleName)
        {
            TRole role = _roleCollection.AsQueryable().SingleOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                _roleCollection.Remove(Query<TRole>.EQ(r => r.Name, roleName));
                return true;
            }
            return false;
        }

        public IFlexMembershipUser CreateOAuthAccount(string provider, string providerUserId, IFlexMembershipUser user)
        {
            var account = new FlexOAuthAccount {Provider = provider, ProviderUserId = providerUserId};
            if (user.OAuthAccounts == null)
            {
                user.OAuthAccounts = new Collection<FlexOAuthAccount>();
            }
            user.OAuthAccounts.Add(account);
            _userCollection.Save(account);

            return user;
        }

        public IFlexMembershipUser GetUserByUsername(string username)
        {
            return _userCollection.AsQueryable().SingleOrDefault(u => u.Username == username);
        }

        public IFlexMembershipUser Add(IFlexMembershipUser user)
        {
            _userCollection.Save(user);
            return user;
        }

        public IFlexMembershipUser Save(IFlexMembershipUser user)
        {
            TUser existingUser = _userCollection.AsQueryable().SingleOrDefault(u => u.Username == user.Username);
            foreach (PropertyInfo property in user.GetType().GetProperties().Where(p => p.CanWrite))
                property.SetValue(existingUser, property.GetValue(user));

            _userCollection.Save(existingUser);
            return user;
        }

        public bool DeleteOAuthAccount(string provider, string providerUserId)
        {
            TUser user =
                _userCollection.AsQueryable().SingleOrDefault(
                    u => u.OAuthAccounts.Any(o => o.ProviderUserId == providerUserId && o.Provider == provider));

            if (user != null)
            {
                if (user.IsLocal || user.OAuthAccounts.Count() > 1)
                {
                    FlexOAuthAccount account =
                        user.OAuthAccounts.Single(o => o.Provider == provider && o.ProviderUserId == providerUserId);
                    user.OAuthAccounts.Remove(account);
                    _userCollection.Save(user);
                    return true;
                }
            }
            return false;
        }

        public IFlexMembershipUser GetUserByOAuthProvider(string provider, string providerUserId)
        {
            return
                _userCollection.AsQueryable().SingleOrDefault(
                    u => u.OAuthAccounts.Any(r => r.Provider == provider && r.ProviderUserId == providerUserId));
        }

        public IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username)
        {
            return _userCollection.AsQueryable()
                .Single(u => u.Username == username)
                .OAuthAccounts.ToArray()
                .Select(o => new OAuthAccount(o.Provider, o.ProviderUserId));
        }

        public IFlexMembershipUser CreateOAuthAccount(string provider, string providerUserId, string username)
        {
            TUser user = _userCollection.AsQueryable().SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                user = new TUser {Username = username};
                _userCollection.Save(user);
            }
            var account = new FlexOAuthAccount {Provider = provider, ProviderUserId = providerUserId};
            if (user.OAuthAccounts == null)
            {
                user.OAuthAccounts = new Collection<FlexOAuthAccount>();
            }
            user.OAuthAccounts.Add(account);
            _userCollection.Save(user);

            return user;
        }
    }
}