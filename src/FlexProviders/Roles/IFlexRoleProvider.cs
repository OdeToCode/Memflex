namespace FlexProviders.Roles
{
    public interface IFlexRoleProvider
    {
        bool IsUserInRole(string username, string roleName, string group = null);
        string[] GetRolesForUser(string username, string group = null);
        void CreateRole(string roleName);
        bool DeleteRole(string roleName, bool throwOnPopulatedRole);
        bool RoleExists(string roleName);
        void AddUsersToRoles(string[] usernames, string[] roleNames, string group = null);
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames, string group = null);
        string[] GetUsersInRole(string roleName, string group = null);
		string[] GetAllUsersInRole(string roleName);
        string[] GetAllRoles();
    }
}