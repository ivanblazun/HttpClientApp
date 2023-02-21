namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumThemesAnswers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        PostId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: false)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Fora",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ThemesCounter = c.Int(nullable: false),
                        UserCounter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Value = c.Int(nullable: false),
                        UserId = c.Int(nullable: true),
                        ForumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fora", t => t.ForumId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ForumId);
            
            AddColumn("dbo.Posts", "Theme_Id", c => c.Int());
            CreateIndex("dbo.Posts", "Theme_Id");
            AddForeignKey("dbo.Posts", "Theme_Id", "dbo.Themes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Themes", "UserId", "dbo.Users");
            DropForeignKey("dbo.Posts", "Theme_Id", "dbo.Themes");
            DropForeignKey("dbo.Themes", "ForumId", "dbo.Fora");
            DropForeignKey("dbo.Answers", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Answers", "UserId", "dbo.Users");
            DropIndex("dbo.Themes", new[] { "ForumId" });
            DropIndex("dbo.Themes", new[] { "UserId" });
            DropIndex("dbo.Posts", new[] { "Theme_Id" });
            DropIndex("dbo.Answers", new[] { "UserId" });
            DropIndex("dbo.Answers", new[] { "PostId" });
            DropColumn("dbo.Posts", "Theme_Id");
            DropTable("dbo.Themes");
            DropTable("dbo.Fora");
            DropTable("dbo.Answers");
        }
    }
}
