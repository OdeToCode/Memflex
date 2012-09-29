using System;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;

namespace FlexProviders.Roles
{
    public class FlexRoleProvider : IFlexRoleProvider
    {
        private readonly IFlexRoleStore _roleStore;

        public FlexRoleProvider(IFlexRoleStore roleStore)
        {
            _roleStore = roleStore;
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return GetUsersInRole(roleName).Any(user => user.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public string[] GetRolesForUser(string username)
        {
            return _roleStore.GetRolesForUser(username);
        }

        public void CreateRole(string roleName)
        {
            _roleStore.CreateRole(roleName);
        }

        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if(_roleStore.GetUsersInRole(roleName).Any() && throwOnPopulatedRole)
            {
                throw new ProviderException(String.Format("Try to delete role {0}, but it is populated", roleName));
            }
            return _roleStore.DeleteRole(roleName);
        }

        public bool RoleExists(string roleName)
        {
            return _roleStore.RoleExists(roleName);
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            _roleStore.AddUsersToRoles(usernames, roleNames);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            _roleStore.RemoveUsersFromRoles(usernames, roleNames);
        }

        public string[] GetUsersInRole(string roleName)
        {
            return _roleStore.GetUsersInRole(roleName);
        }

        public string[] GetAllRoles()
        {
            return _roleStore.GetAllRoles();
        }
    }
}