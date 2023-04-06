namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateandtime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MainForums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ForumsCounter = c.Int(nullable: false),
                        UserCounter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Answers", "TimeAnswerCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Posts", "TimePostCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Themes", "TimeThemeCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Fora", "TimeForumCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fora", "TimeForumCreated");
            DropColumn("dbo.Themes", "TimeThemeCreated");
            DropColumn("dbo.Posts", "TimePostCreated");
            DropColumn("dbo.Answers", "TimeAnswerCreated");
            DropTable("dbo.MainForums");
        }
    }
}
