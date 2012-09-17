namespace FlexProviders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OAuthAccounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EfOAuthAccounts",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 128),
                        ProviderUserId = c.String(nullable: false, maxLength: 128),
                        EfUser_Id = c.Int(),
                    })
                .PrimaryKey(t => new {t.Provider, t.ProviderUserId})
                .ForeignKey("dbo.EfUsers", t => t.EfUser_Id)
                .Index(t => t.EfUser_Id);
        }
        
        public override void Down()
        {
            DropIndex("dbo.EfOAuthAccounts", new[] { "EfUser_Id" });
            DropForeignKey("dbo.EfOAuthAccounts", "EfUser_Id", "dbo.EfUsers");
            DropTable("dbo.EfOAuthAccounts");
        }
    }
}
