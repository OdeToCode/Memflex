namespace FlexProviders.Roles
{
    public interface IFlexRoleStore
    {
        void CreateRole(string roleName);
        string[] GetRolesForUser(string username);
        string[] GetUsersInRole(string roleName);
        string[] GetAllRoles();
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames);
        void AddUsersToRoles(string[] usernames, string[] roleNames);
        bool RoleExists(string roleName);
        bool DeleteRole(string roleName);
    }
}