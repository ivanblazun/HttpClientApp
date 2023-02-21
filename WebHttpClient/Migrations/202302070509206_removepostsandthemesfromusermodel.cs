namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removepostsandthemesfromusermodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "UserId", "dbo.Users");
            DropIndex("dbo.Posts", new[] { "UserId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Posts", "UserId");
            AddForeignKey("dbo.Posts", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
