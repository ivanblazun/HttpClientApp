namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addThemeIdAndFKToPosts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "Theme_Id", "dbo.Themes");
            DropIndex("dbo.Posts", new[] { "Theme_Id" });
            RenameColumn(table: "dbo.Posts", name: "Theme_Id", newName: "ThemeId");
            AlterColumn("dbo.Posts", "ThemeId", c => c.Int(nullable: true));
            CreateIndex("dbo.Posts", "ThemeId");
            AddForeignKey("dbo.Posts", "ThemeId", "dbo.Themes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ThemeId", "dbo.Themes");
            DropIndex("dbo.Posts", new[] { "ThemeId" });
            AlterColumn("dbo.Posts", "ThemeId", c => c.Int());
            RenameColumn(table: "dbo.Posts", name: "ThemeId", newName: "Theme_Id");
            CreateIndex("dbo.Posts", "Theme_Id");
            AddForeignKey("dbo.Posts", "Theme_Id", "dbo.Themes", "Id");
        }
    }
}
