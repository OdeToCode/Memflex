using FlexProviders.EF;

namespace FlexProviders.Tests.Integration
{
    public class IntegrationTest
    {
        protected readonly FlexMemebershipProvider _provider;
        protected readonly FakeApplicationEnvironment _environment;
        protected readonly EfUserRepository _repository;
        protected TestDb _db;

        public IntegrationTest()
        {
            _db = new TestDb();
            _repository = new DefaultUserRepository();            
            _environment = new FakeApplicationEnvironment();
            _provider = new FlexMemebershipProvider(_repository,_repository, _environment);
        }
 
    }
}