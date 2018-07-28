using System;
using System.Configuration.Provider;
using System.Linq;

namespace FlexProviders.Roles
{
    public class FlexRoleProvider : IFlexRoleProvider
    {
        private readonly IFlexRoleStore _roleStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexRoleProvider" /> class.
        /// </summary>
        /// <param name="roleStore">The role store.</param>
        public FlexRoleProvider(IFlexRoleStore roleStore)
        {
            _roleStore = roleStore;
        }

		/// <summary>
		/// Determines whether the specified <paramref name="username" /> is in
		/// the specified <paramref name="roleName" />.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="roleName">Name of the role.</param>
		/// <param name="license">The license the user belongs to.</param>
		/// <returns>
		/// <c>true</c> if the username is specified role;
		/// otherwise, <c>false</c> .
		/// </returns>
		public bool IsUserInRole(string username, string roleName, string license = null)
        {
            return GetUsersInRole(roleName, license).Any(user => user.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

		/// <summary>
		/// Gets the roles for user.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="license">The license the user belongs to.</param>
		/// <returns></returns>
		public string[] GetRolesForUser(string username, string license = null)
        {
            return _roleStore.GetRolesForUser(username, license);
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        public void CreateRole(string roleName)
        {
            _roleStore.CreateRole(roleName);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="throwOnPopulatedRole">if set to <c>true</c> [throw on populated role].</param>
        /// <returns></returns>
        /// <exception cref="System.Configuration.Provider.ProviderException"></exception>
        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if(_roleStore.GetAllUsersInRole(roleName).Any() && throwOnPopulatedRole)
            {
                throw new ProviderException(String.Format("Try to delete role {0}, but it is populated", roleName));
            }
            return _roleStore.DeleteRole(roleName);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="roleName" /> exists
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public bool RoleExists(string roleName)
        {
            return _roleStore.RoleExists(roleName);
        }


		/// <summary>
		/// Adds the users to roles.
		/// </summary>
		/// <param name="usernames">The usernames to add the to the roles.</param>
		/// <param name="roleNames">The role names to add the users to.</param>
		/// <param name="license">The license the users belong to.</param>
		public void AddUsersToRoles(string[] usernames, string[] roleNames, string license = null)
        {
            _roleStore.AddUsersToRoles(usernames, roleNames, license);
        }

		/// <summary>
		/// Removes the users from roles.
		/// </summary>
		/// <param name="usernames">The usernames.</param>
		/// <param name="roleNames">The role names.</param>
		/// <param name="license">The license the users belong to.</param>
		public void RemoveUsersFromRoles(string[] usernames, string[] roleNames, string license = null)
        {
            _roleStore.RemoveUsersFromRoles(usernames, roleNames, license);
        }

		/// <summary>
		/// Gets the users in role.
		/// </summary>
		/// <param name="roleName">Name of the role.</param>
		/// <param name="license">The license the users belong to.</param>
		/// <returns>
		/// 
		/// </returns>
		public string[] GetUsersInRole(string roleName, string license = null)
        {
            return _roleStore.GetUsersInRole(roleName, license);
        }

		/// <summary>
		/// Gets the all users in role.
		/// </summary>
		/// <param name="roleName">Name of the role.</param>
		/// <returns>
		/// 
		/// </returns>
		public string[] GetAllUsersInRole(string roleName)
		{
			return _roleStore.GetAllUsersInRole(roleName);
		}

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public string[] GetAllRoles()
        {
            return _roleStore.GetAllRoles();
        }
    }
}