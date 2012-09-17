using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.EF
{
    public class EfUserRepository : DbContext, IFlexUserRepository, IFlexOAuthUserRepository
    {
        public EfUserRepository() {}

        public EfUserRepository(string nameorConnectionString) : base(nameorConnectionString) {}

        public DbSet<EfUser> Users { get; set; }

        public IFlexMembershipUser GetUserByUsername(string username)
        {
            return Users.SingleOrDefault(u => u.Username == username);
        }
        
        public IFlexMembershipUser Add(IFlexMembershipUser user)
        {
            Users.Add(user as EfUser);
            SaveChanges();
            return user;
        }

        public IFlexMembershipUser Save(IFlexMembershipUser user)
        {
            Entry(user).State = EntityState.Modified;
            SaveChanges();
            return user;
        }

        public IFlexOAuthUser CreateOAuthAccount(string provider, string providerUserId, string username)
        {
            var user = Users.Include(u => u.OAuthAccounts).SingleOrDefault(u => u.Username == username);
            if(user == null)
            {
                user = new EfUser();
                user.Username = username;
                Users.Add(user);
            }
            var account = new EfOAuthAccount() {Provider = provider, ProviderUserId = providerUserId};
            user.OAuthAccounts.Add(account);
            SaveChanges();
            
            return user;
        }

        public IFlexOAuthUser GetUserByOAuthProvider(string provider, string providerUserId)
        {
            var user =
                Users.SingleOrDefault(
                    u => u.OAuthAccounts.Any(a => a.Provider == provider && a.ProviderUserId == providerUserId));
            return user;
        }

        public bool DeleteOAuthAccount(string provider, string providerUserId)
        {
            var user = Users.SingleOrDefault(u => u.OAuthAccounts.Any(a => a.Provider == provider && a.ProviderUserId == providerUserId));
            if(user.IsLocal || user.OAuthAccounts.Count > 1)
            {
                var account = user.OAuthAccounts.Single(a => a.Provider == provider && a.ProviderUserId == providerUserId);
                user.OAuthAccounts.Remove(account);
                SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username)
        {
            var user = Users.Single(u => u.Username == username);
            return user.OAuthAccounts.Select(
                account => new OAuthAccount(account.Provider, account.ProviderUserId));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EfOAuthAccount>()
                        .HasKey(a => new {a.Provider, a.ProviderUserId});
            base.OnModelCreating(modelBuilder);
        }
    }
}