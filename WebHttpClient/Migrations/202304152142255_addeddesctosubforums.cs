namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeddesctosubforums : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fora", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fora", "Description");
        }
    }
}
