using System.Data.Entity;
using FlexProviders.Membership;

namespace LogMeIn.Models
{
    public class MovieDb : DbContext
    {
        public MovieDb()
        {
        }

        public MovieDb(string connectionStringOrName)
            : base(connectionStringOrName)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlexOAuthAccount>().HasKey(a => new { a.Provider, a.ProviderUserId });
            base.OnModelCreating(modelBuilder);
        }
    }
}