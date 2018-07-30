using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using FlexProviders.Membership;
using FlexProviders.Roles;

namespace FlexProviders.EF
{
    public class FlexRoleStore<TRole, TUser> : IFlexRoleStore
        where TRole : class, IFlexRole<TUser>, new()
        where TUser : class, IFlexMembershipUser
    {
        private readonly DbContext _context;

        public FlexRoleStore(DbContext context)
        {
            _context = context;
        }

        public void CreateRole(string roleName)
        {
            var role = new TRole {Name = roleName};
            _context.Set<TRole>().Add(role);
            _context.SaveChanges();
        }

        public string[] GetRolesForUser(string username, string license = null)
        {
			if(license == null)
			{
				return _context.Set<TRole>().Where(role => role.Users.Any(u => u.Username.Equals(username) && u.License == null))
						   .Select(role => role.Name).ToArray();
			}

            return _context.Set<TRole>().Where(role => role.Users.Any(u => u.Username.Equals(username) && u.License == license))
                           .Select(role => role.Name).ToArray();
        }

        public string[] GetUsersInRole(string roleName, string license = null)
        {
			if (license == null)
			{
				return _context.Set<TRole>().Where(role => role.Name.Equals(roleName))
				.SelectMany(role => role.Users.Where(u => u.License == null)).Select(user => user.Username)
						   .ToArray();
			}

			return _context.Set<TRole>().Where(role => role.Name.Equals(roleName))
                .SelectMany(role => role.Users.Where(u => u.License == license)).Select(user => user.Username)
                           .ToArray();

        }
		public string[] GetAllUsersInRole(string roleName)
		{
			return _context.Set<TRole>().Where(role => role.Name.Equals(roleName))
				.SelectMany(role => role.Users).Select(user => user.Username)
						   .ToArray();
		}

        public string[] GetAllRoles()
        {
            return _context.Set<TRole>().Select(role => role.Name).ToArray();
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch, string license = null)
        {
			if (license == null)
			{
				return _context.Set<TRole>().Where(role => role.Name.Equals(roleName))
				.SelectMany(role => role.Users.Where(user => user.License == null)).Where(user => user.Username.StartsWith(usernameToMatch)).Select(user => user.Username)
						  .ToArray();
			}

			return _context.Set<TRole>().Where(role => role.Name.Equals(roleName))
                .SelectMany(role => role.Users.Where(user => user.License == license)).Where(user => user.Username.StartsWith(usernameToMatch)).Select(user => user.Username)
                          .ToArray();
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames, string license = null)
        {
			List<TUser> users = null;

			if (license == null)
			{
				users = _context.Set<TUser>().Where(u => usernames.Contains(u.Username) && u.License == null).ToList();
			}
			else
			{
				users = _context.Set<TUser>().Where(u => usernames.Contains(u.Username) && u.License == license).ToList();
			}

			foreach (var roleName in roleNames)
            {
                var role = _context.Set<TRole>().Include(r=>r.Users).SingleOrDefault(r => r.Name == roleName);
                if (role != null)
                {
                    foreach (var user in users)
                    {
                        role.Users.Remove(user);
                    }
                }
            }
            _context.SaveChanges();
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames, string license = null)
        {
			List<TUser> users = null;

			if (license == null)
			{
				users = _context.Set<TUser>().Where(u => usernames.Contains(u.Username) && u.License == null).ToList();
			}
			else
			{
				users = _context.Set<TUser>().Where(u => usernames.Contains(u.Username) && u.License == license).ToList();
			}

			foreach (var roleName in roleNames)
            {
                var role = _context.Set<TRole>().SingleOrDefault(r => r.Name == roleName);
                if (role != null)
                {
                    if(role.Users == null)
                    {
                        role.Users = new Collection<TUser>();
                    }
                    foreach (var user in users)
                    {
                        role.Users.Add(user);
                    }
                }
            }
            _context.SaveChanges();
        }

        public bool RoleExists(string roleName)
        {
            return _context.Set<TRole>().Any(r => r.Name == roleName);
        }

        public bool DeleteRole(string roleName)
        {
            var role = _context.Set<TRole>().Include(r=>r.Users).SingleOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                role.Users.Clear();
                _context.Set<TRole>().Remove(role);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}