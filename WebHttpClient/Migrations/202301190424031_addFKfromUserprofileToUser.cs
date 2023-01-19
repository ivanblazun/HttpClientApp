namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFKfromUserprofileToUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfiles", "User_Id", "dbo.Users");
            DropIndex("dbo.UserProfiles", new[] { "User_Id" });
            RenameColumn(table: "dbo.UserProfiles", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.UserProfiles", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserProfiles", "UserId");
            AddForeignKey("dbo.UserProfiles", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfiles", "UserId", "dbo.Users");
            DropIndex("dbo.UserProfiles", new[] { "UserId" });
            AlterColumn("dbo.UserProfiles", "UserId", c => c.Int());
            RenameColumn(table: "dbo.UserProfiles", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.UserProfiles", "User_Id");
            AddForeignKey("dbo.UserProfiles", "User_Id", "dbo.Users", "Id");
        }
    }
}
