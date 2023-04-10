namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Themes", "UserId", "dbo.Users");
            DropIndex("dbo.Themes", new[] { "UserId" });
            AlterColumn("dbo.Themes", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Themes", "UserId");
            AddForeignKey("dbo.Themes", "UserId", "dbo.Users", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Themes", "UserId", "dbo.Users");
            DropIndex("dbo.Themes", new[] { "UserId" });
            AlterColumn("dbo.Themes", "UserId", c => c.Int());
            CreateIndex("dbo.Themes", "UserId");
            AddForeignKey("dbo.Themes", "UserId", "dbo.Users", "Id");
        }
    }
}
