using System.Data.Entity;
using FlexProviders.Membership;

namespace FlexProviders.Tests.Integration.EF
{
    public class SomeDb : DbContext
    {
        public SomeDb()
        {
        }

        public SomeDb(string connectionStringOrName)
            : base(connectionStringOrName)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlexOAuthAccount>().HasKey(a => new { a.Provider, a.ProviderUserId });
            base.OnModelCreating(modelBuilder);
        }
    }
}