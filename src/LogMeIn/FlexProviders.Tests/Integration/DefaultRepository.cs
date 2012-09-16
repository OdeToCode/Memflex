using FlexProviders.EF;

namespace FlexProviders.Tests.Integration
{
    public class DefaultUserRepository : EfUserRepository
    {
        public DefaultUserRepository() :base("name=Default")
        {
            
        }
    }
}