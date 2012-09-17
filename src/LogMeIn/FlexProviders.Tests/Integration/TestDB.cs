using System;
using System.Data.Entity;
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
            var forcedResultToCreateDatabaseHere = new DefaultUserRepository().Users.ToList();            
        }

        public bool CanFindUsername(string username)
        {
            return _db.EfUsers.FindByUsername(username) != null;
        }

        public string GetPassword(string username)
        {
            return _db.EfUsers.FindByUsername(username).Password;
        }

        public int GetCountOfOAuthAccounts(string username)
        {
            var userId = _db.EfUsers.FindByUsername(username).Id;
            return _db.EfOAuthAccounts.FindAll(_db.EfOAuthAccounts.Ef_UserId == userId).Count();
        }

        private readonly dynamic _db;
        private readonly static Lazy<dynamic> _initializer = new Lazy<dynamic>(Initialize);
    }
}