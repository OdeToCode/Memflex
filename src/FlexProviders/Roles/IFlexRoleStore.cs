namespace FlexProviders.Roles
{
    public interface IFlexRoleStore
    {
        void CreateRole(string roleName);
        string[] GetRolesForUser(string username, string group = null);
        string[] GetUsersInRole(string roleName, string group = null);
		string[] GetAllUsersInRole(string roleName);
        string[] GetAllRoles();
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames, string group = null);
        void AddUsersToRoles(string[] usernames, string[] roleNames, string group = null);
        bool RoleExists(string roleName);
        bool DeleteRole(string roleName);
    }
}