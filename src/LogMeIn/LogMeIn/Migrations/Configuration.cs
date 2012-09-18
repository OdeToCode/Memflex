namespace LogMeIn.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<LogMeIn.Models.MovieDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
