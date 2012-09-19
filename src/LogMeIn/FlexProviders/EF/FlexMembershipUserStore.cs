using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.EF
{
    public class FlexMembershipUserStore<TUser, TContext> 
        : IFlexUserStore, IFlexOAuthDataStore 
            where TUser: class, IFlexMembershipUser, new()             
            where TContext : DbContext
    {
        private readonly TContext _context;

        public FlexMembershipUserStore (TContext context)
        {
            _context = context;
        }
                    
        public IFlexMembershipUser GetUserByUsername(string username)
        {
            return _context.Set<TUser>().SingleOrDefault(u => u.Username == username);
        }

        public IFlexMembershipUser Add(IFlexMembershipUser user)
        {
            _context.Set<TUser>().Add((TUser)user);
            _context.SaveChanges();
            return user;
        }

        public IFlexMembershipUser Save(IFlexMembershipUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
           return user;
        }

        public IFlexMembershipUser CreateOAuthAccount(string provider, string providerUserId, string username)
        {
            var users = _context.Set<TUser>();
            var user = users.Include(u => u.OAuthAccounts)
                            .SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                user = new TUser { Username = username };
                users.Add(user);
            }
            var account = new FlexOAuthAccount() { Provider = provider, ProviderUserId = providerUserId };
            if(user.OAuthAccounts == null)
            {
                user.OAuthAccounts = new Collection<FlexOAuthAccount>();
            }
            user.OAuthAccounts.Add(account);
            _context.SaveChanges();

            return user;
        }

        public IFlexMembershipUser GetUserByOAuthProvider(string provider, string providerUserId)
        {
            var user = _context.Set<TUser>()
                               .SingleOrDefault(u => u.OAuthAccounts.Any(a => a.Provider == provider && a.ProviderUserId == providerUserId));
            return user;
        }

        public bool DeleteOAuthAccount(string provider, string providerUserId)
        {
            var user = _context.Set<TUser>().SingleOrDefault(u => u.OAuthAccounts.Any(a => a.Provider == provider && a.ProviderUserId == providerUserId));
            if (user.IsLocal || user.OAuthAccounts.Count > 1)
            {
                var account = user.OAuthAccounts.Single(a => a.Provider == provider && a.ProviderUserId == providerUserId);
                user.OAuthAccounts.Remove(account);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username)
        {
            var user = _context.Set<TUser>().Single(u => u.Username == username);
            return user.OAuthAccounts.Select(account => new OAuthAccount(account.Provider, account.ProviderUserId));
        }
    }
}