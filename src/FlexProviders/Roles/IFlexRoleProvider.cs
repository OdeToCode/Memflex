namespace FlexProviders.Roles
{
    public interface IFlexRoleProvider
    {
        bool IsUserInRole(string username, string roleName, string license = null);
        string[] GetRolesForUser(string username, string license = null);
        void CreateRole(string roleName);
        bool DeleteRole(string roleName, bool throwOnPopulatedRole);
        bool RoleExists(string roleName);
        void AddUsersToRoles(string[] usernames, string[] roleNames, string license = null);
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames, string license = null);
        string[] GetUsersInRole(string roleName, string license = null);
		string[] GetAllUsersInRole(string roleName);
        string[] GetAllRoles();
    }
}