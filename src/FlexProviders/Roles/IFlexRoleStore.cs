namespace FlexProviders.Roles
{
    public interface IFlexRoleStore
    {
        void CreateRole(string roleName);
        string[] GetRolesForUser(string username, string license = null);
        string[] GetUsersInRole(string roleName, string license = null);
		string[] GetAllUsersInRole(string roleName);
        string[] GetAllRoles();
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames, string license = null);
        void AddUsersToRoles(string[] usernames, string[] roleNames, string license = null);
        bool RoleExists(string roleName);
        bool DeleteRole(string roleName);
    }
}