using System;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;

namespace FlexProviders.Roles
{
    public class FlexRoleProvider : RoleProvider
    {
        private IFlexRoleStore _roleStore;

        public FlexRoleProvider Initialize(IFlexRoleStore roleStore)
        {
            _roleStore = roleStore;
            return this;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return GetUsersInRole(roleName).Any(user => user.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public override string[] GetRolesForUser(string username)
        {
            return _roleStore.GetRolesForUser(username);
        }

        public override void CreateRole(string roleName)
        {
            _roleStore.CreateRole(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if(_roleStore.GetUsersInRole(roleName).Any() && throwOnPopulatedRole)
            {
                throw new ProviderException(String.Format("Try to delete role {0}, but it is populated", roleName));
            }
            return _roleStore.DeleteRole(roleName);
        }

        public override bool RoleExists(string roleName)
        {
            return _roleStore.RoleExists(roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            _roleStore.AddUsersToRoles(usernames, roleNames);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            _roleStore.RemoveUsersFromRoles(usernames, roleNames);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return _roleStore.GetUsersInRole(roleName);
        }

        public override string[] GetAllRoles()
        {
            return _roleStore.GetAllRoles();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return _roleStore.FindUsersInRole(roleName, usernameToMatch);
        }

        public override string ApplicationName { get; set; }
    }
}