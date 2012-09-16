using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using FlexProviders.EF;

namespace FlexProviders.Tests.Integration
{
    public class TestDb 
    {
        public TestDb()
        {
            _db = _initializer.Value;
        }

        protected static dynamic Initialize()
        {
            UseEntityFrameworkToCreateDatabase();
            return Simple.Data.Database.OpenNamedConnection("Default");
        }

        private static void UseEntityFrameworkToCreateDatabase()
        {
            Database.Delete("Default");
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfUserRepository, Migrations.Configuration>("Default"));            
            new DefaultUserRepository().Users.ToList();
        }

        public bool CanFindUsername(string username)
        {
            return _db.Value.EfUsers.FindByUsername(username) != null;
        }

        private readonly dynamic _db;
        private readonly static Lazy<dynamic> _initializer = new Lazy<dynamic>(Initialize);
    }
}