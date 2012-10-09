using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.DataClasses;
using System.Linq;
using FlexProviders.Membership;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.EF
{
    public class FlexMembershipUserStore<TUser> 
        : IFlexUserStore
            where TUser: class, IFlexMembershipUser, new()             
    {
        private readonly DbContext _context;

        public FlexMembershipUserStore (DbContext context)
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

        public IFlexMembershipUser CreateOAuthAccount(string provider, string providerUserId, IFlexMembershipUser user)
        {
            user = _context.Set<TUser>().Single(u => u.Username == user.Username);
            if(user.OAuthAccounts == null)
            {
                user.OAuthAccounts = new EntityCollection<FlexOAuthAccount>();
            }
            user.OAuthAccounts.Add(new FlexOAuthAccount() { Provider = provider, ProviderUserId = providerUserId});
            _context.SaveChanges();
            return user;
        }

        public IFlexMembershipUser GetUserByOAuthProvider(string provider, string providerUserId)
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

        public IFlexMembershipUser GetUserByPasswordResetToken(string passwordResetToken)
        {
            var user = _context.Set<TUser>().SingleOrDefault(u => u.PasswordResetToken == passwordResetToken);
            return user;
        }

        public IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username)
        {
            var user = _context.Set<TUser>().Single(u => u.Username == username);
            return user.OAuthAccounts.Select(account => new OAuthAccount(account.Provider, account.ProviderUserId));
        }
    }
}