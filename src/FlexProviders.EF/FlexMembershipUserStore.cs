using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text.RegularExpressions;
using FlexProviders.Membership;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.EF
{
    public class FlexMembershipUserStore<TUser> : 
            IFlexUserStore<TUser>
            where TUser: class, IFlexMembershipUser, new()             
    {
        private readonly DbContext _context;

        public FlexMembershipUserStore (DbContext context)
        {
            _context = context;
        }
                    
        public TUser GetUserByUsername(string username, string license = null)
        {
            return _context.Set<TUser>().SingleOrDefault(u => u.Username == username && u.License == license);
        }

		public TUser GetUserBySsoToken(string token)
		{
			var utcNow = DateTime.UtcNow;
			return _context.Set<TUser>().SingleOrDefault(u => u.SsoAccessToken != null && u.SsoAccessToken == token && u.SsoTokenExpiration.HasValue && u.SsoTokenExpiration.Value > utcNow);
		}

	    public IEnumerable<TUser> GetAllUsers(string license = null)
	    {
			return _context.Set<TUser>().Where(u => u.License == license).ToList();
	    }

        public TUser Add(TUser user)
        {
            _context.Set<TUser>().Add((TUser)user);
            _context.SaveChanges();
            return user;
        }

        public TUser Save(TUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
           return user;
        }

        public TUser CreateOAuthAccount(string provider, string providerUserId, TUser user)
        {
            user = _context.Set<TUser>().Single(u => u.Username == user.Username && u.License == user.License);
            if(user.OAuthAccounts == null)
            {
                user.OAuthAccounts = new EntityCollection<FlexOAuthAccount>();
            }
            user.OAuthAccounts.Add(new FlexOAuthAccount() { Provider = provider, ProviderUserId = providerUserId});
            _context.SaveChanges();
            return user;
        }

        public TUser GetUserByOAuthProvider(string provider, string providerUserId)
        {
            var user = _context.Set<TUser>().SingleOrDefault(u => u.OAuthAccounts.Any(a => a.Provider == provider && a.ProviderUserId == providerUserId));
            return user;
        }

        public bool DeleteOAuthAccount(string provider, string providerUserId)
        {            
            var account = _context.Set<FlexOAuthAccount>().Find(provider, providerUserId);
            if(account != null)
            {
                _context.Set<FlexOAuthAccount>().Remove(account);
                _context.SaveChanges();
                return true;
            }            
            return false;
        }

        public TUser GetUserByPasswordResetToken(string passwordResetToken)
        {
            var user = _context.Set<TUser>().SingleOrDefault(u => u.PasswordResetToken == passwordResetToken);
            return user;
        }

        public IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username, string license = null)
        {
            var user = _context.Set<TUser>().Single(u => u.Username == username && u.License == license);
            return user.OAuthAccounts.Select(account => new OAuthAccount(account.Provider, account.ProviderUserId));
        }

		/// <summary>
		/// Renames a license by taking all users of the old license and moving them to the new license.
		/// Will return false if users exist with the new name
		/// </summary>
		/// <param name="oldName">The current name you want to change away from.</param>
		/// <param name="newName">The new license name that all users will be linked to.</param>
		public bool RenameLicense(string oldName, string newName)
		{
			//There are already users with the new license name so we do nothing
			if (_context.Set<TUser>().Count(u => u.License == newName) > 0)
			{
				return false;
			}

			var users = _context.Set<TUser>().Where(u => u.License == oldName);

			foreach (var user in users)
			{
				user.License = newName;
			}
			_context.SaveChanges();

			return true;
		}
    }
}