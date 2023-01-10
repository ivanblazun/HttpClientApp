namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserStatusSEED : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO UserStatus (StatusTitle,UStatus,UserId) VALUES ('User','3','3');");
            Sql("INSERT INTO UserStatus (StatusTitle,UStatus,UserId) VALUES ('PowerUser','2','2');");
            Sql("INSERT INTO UserStatus (StatusTitle,UStatus,UserId) VALUES ('Admin','1','1');");
        }

        public override void Down()
        {
        }
    }
}
