namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfileUserRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "UserProfile_Id", "dbo.UserProfiles");
            DropIndex("dbo.Users", new[] { "UserProfile_Id" });
            AddColumn("dbo.UserProfiles", "User_Id", c => c.Int());
            CreateIndex("dbo.UserProfiles", "User_Id");
            AddForeignKey("dbo.UserProfiles", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Users", "UserProfile_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserProfile_Id", c => c.Int());
            DropForeignKey("dbo.UserProfiles", "User_Id", "dbo.Users");
            DropIndex("dbo.UserProfiles", new[] { "User_Id" });
            DropColumn("dbo.UserProfiles", "User_Id");
            CreateIndex("dbo.Users", "UserProfile_Id");
            AddForeignKey("dbo.Users", "UserProfile_Id", "dbo.UserProfiles", "Id");
        }
    }
}
