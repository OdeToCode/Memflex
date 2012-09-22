using System.Collections.Generic;
using System.Data.Entity;
using FlexProviders;
using FlexProviders.EF;
using FlexProviders.Membership;

namespace LogMeIn.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }
    }

    public class User : IFlexMembershipUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocal { get; set; }
        public int FavoriteNumber { get; set; }
        public virtual ICollection<FlexOAuthAccount> OAuthAccounts { get; set; }
        public ICollection<Role> Roles { get; set; }
    }

    public class Role : IFlexRole<User>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }

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