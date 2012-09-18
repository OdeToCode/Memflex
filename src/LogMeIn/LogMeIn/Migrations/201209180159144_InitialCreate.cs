namespace LogMeIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Length = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        IsLocal = c.Boolean(nullable: false),
                        FavoriteNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FlexOAuthAccounts",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 128),
                        ProviderUserId = c.String(nullable: false, maxLength: 128),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.Provider, t.ProviderUserId })
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FlexOAuthAccounts", new[] { "User_Id" });
            DropForeignKey("dbo.FlexOAuthAccounts", "User_Id", "dbo.Users");
            DropTable("dbo.FlexOAuthAccounts");
            DropTable("dbo.Users");
            DropTable("dbo.Movies");
        }
    }
}
