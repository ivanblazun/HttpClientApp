namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusTitle = c.String(),
                        UStatus = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Users", "UserStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserStatus", "UserId", "dbo.Users");
            DropIndex("dbo.UserStatus", new[] { "UserId" });
            DropColumn("dbo.Users", "UserStatus");
            DropTable("dbo.UserStatus");
        }
    }
}
