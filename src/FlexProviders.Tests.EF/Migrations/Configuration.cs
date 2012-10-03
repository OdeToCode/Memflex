using System.Data.Entity.Migrations;

namespace FlexProviders.Tests.Integration.EF.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<SomeDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
