namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userProfileAndUserRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Avatar = c.String(),
                        AboutMyself = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "UserProfile_Id", c => c.Int());
            AddColumn("dbo.Posts", "AnswerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "UserProfile_Id");
            AddForeignKey("dbo.Users", "UserProfile_Id", "dbo.UserProfiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserProfile_Id", "dbo.UserProfiles");
            DropIndex("dbo.Users", new[] { "UserProfile_Id" });
            DropColumn("dbo.Posts", "AnswerId");
            DropColumn("dbo.Users", "UserProfile_Id");
            DropTable("dbo.UserProfiles");
        }
    }
}
