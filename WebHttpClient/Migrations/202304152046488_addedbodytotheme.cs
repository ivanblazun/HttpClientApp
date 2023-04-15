namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedbodytotheme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Themes", "ThemeBody", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Themes", "ThemeBody");
        }
    }
}
