using System;
using System.Data.Entity;

namespace FlexProviders.EF
{
    public class EfUserRepository : DbContext, IFlexUserRepository
    {        
        public DbSet<EfUser> Users { get; set; }

        public IFlexMembershipUser GetUserByUsername(string username)
        {
            throw new System.NotImplementedException();
        }

        public IFlexMembershipUser Add(IFlexMembershipUser user)
        {
            throw new System.NotImplementedException();
        }

        public IFlexMembershipUser Save(IFlexMembershipUser user)
        {
            throw new NotImplementedException();
        }
    }
}