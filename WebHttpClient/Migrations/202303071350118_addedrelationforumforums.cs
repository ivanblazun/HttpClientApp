namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedrelationforumforums : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fora", "MainForumId", c => c.Int());
            CreateIndex("dbo.Fora", "MainForumId");
            AddForeignKey("dbo.Fora", "MainForumId", "dbo.MainForums", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fora", "MainForumId", "dbo.MainForums");
            DropIndex("dbo.Fora", new[] { "MainForumId" });
            DropColumn("dbo.Fora", "MainForumId");
        }
    }
}
