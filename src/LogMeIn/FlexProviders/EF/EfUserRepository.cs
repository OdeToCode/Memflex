using System;
using System.Data.Entity;
using System.Linq;

namespace FlexProviders.EF
{
    public class EfUserRepository : DbContext, IFlexUserRepository, IFlexOAuthUserRepository
    {
        public EfUserRepository() 
        {
            
        }

        public EfUserRepository(string nameorConnectionString) : base(nameorConnectionString)
        {
            
        }

        public DbSet<EfUser> Users { get; set; }

        public IFlexMembershipUser GetUserByUsername(string username)
        {
            return Users.SingleOrDefault(u => u.Username == username);
        }
        
        public IFlexMembershipUser Add(IFlexMembershipUser user)
        {
            Users.Add((EfUser)user);
            SaveChanges();
            return user;
        }

        public IFlexMembershipUser Save(IFlexMembershipUser user)
        {
            throw new NotImplementedException();
        }

        public IFlexOAuthUser GetUserByOAuthProvider(string provider, string providerUserId)
        {
            throw new NotImplementedException();
        }

        public IFlexOAuthUser DeleteOAuthAccount(string provider, string providerUserId)
        {
            throw new NotImplementedException();
        }

        IFlexOAuthUser IFlexOAuthUserRepository.GetUserByUsername(string ownerAccount)
        {
            throw new NotImplementedException();
        }

        public IFlexOAuthUser CreateOrUpdate(string provider, string providerUserId, string username)
        {
            throw new NotImplementedException();
        }
    }
}