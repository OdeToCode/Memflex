using FlexProviders;

namespace LogMeIn.Tests.Integration
{
    public class IntegrationTest
    {
        protected readonly FlexMemebershipProvider _provider;
        protected readonly FakeApplicationEnvironment _environment;
        protected readonly DefaultUserStore Store;
        protected TestDb _db;

        public IntegrationTest()
        {
            _db = new TestDb();
            Store = new DefaultUserStore();            
            _environment = new FakeApplicationEnvironment();
            _provider = new FlexMemebershipProvider(Store,Store, _environment);
        }
 
    }
}