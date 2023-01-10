namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddedPasswordAndDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "RegisteredDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RegisteredDate");
            DropColumn("dbo.Users", "Email");
        }
    }
}
