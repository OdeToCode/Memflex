namespace FlexProviders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EfUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 256),
                        Password = c.String(maxLength: 128),
                        Salt = c.String(maxLength: 128),
                        IsLocal = c.Boolean(nullable: false),
                        Provider = c.String(maxLength: 128),
                        ProviderUsername = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(e => e.Username, unique: true)
                .Index(e => new {e.Provider, e.ProviderUsername});                        
        }
        
        public override void Down()
        {
            DropTable("dbo.EfUsers");
        }
    }
}
