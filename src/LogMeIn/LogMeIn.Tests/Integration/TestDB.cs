using System;
using System.Data.Entity;
using System.Linq;
using FlexProviders.EF;
using LogMeIn.Migrations;
using LogMeIn.Models;

namespace LogMeIn.Tests.Integration
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
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MovieDb, Configuration>("Default"));            
            var forcedResultToCreateDatabaseHere = new MovieDb("name=Default").Users.ToList();            
        }

        public bool CanFindUsername(string username)
        {
            return _db.Users.FindByUsername(username) != null;
        }

        public string GetPassword(string username)
        {
            return _db.Users.FindByUsername(username).Password;
        }

        public int GetCountOfOAuthAccounts(string username)
        {
            var userId = _db.Users.FindByUsername(username).Id;
            return _db.FlexOAuthAccounts.FindAll(_db.FlexOAuthAccounts.User_Id == userId).Count();
        }

        private readonly dynamic _db;
        private readonly static Lazy<dynamic> _initializer = new Lazy<dynamic>(Initialize);
    }
}