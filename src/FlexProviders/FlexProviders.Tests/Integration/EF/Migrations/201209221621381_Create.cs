using System.Data.Entity.Migrations;

namespace FlexProviders.Tests.Integration.EF.Migrations
{
    public partial class Create : DbMigration
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
                        Username = c.String(maxLength:128),
                        Password = c.String(maxLength:128),
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

            CreateTable(
                "dbo.Roles",
                c => new
                         {
                             Id = c.Int(nullable: false, identity: true),
                             Name = c.String(nullable:false, maxLength:128),
                         })
                .PrimaryKey(t => t.Id)
                .Index(r => r.Name);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.FlexOAuthAccounts", new[] { "User_Id" });
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.FlexOAuthAccounts", "User_Id", "dbo.Users");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Roles");
            DropTable("dbo.FlexOAuthAccounts");
            DropTable("dbo.Users");
            DropTable("dbo.Movies");
        }
    }
}
