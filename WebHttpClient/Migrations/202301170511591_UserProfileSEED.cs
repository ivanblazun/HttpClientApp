namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfileSEED : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO UserProfiles (FirstName,LastName,Avatar,AboutMySelf,User_Id) VALUES ('Ivan','Blazun','Avatar Ivan','About Ivan','1');");
            Sql("INSERT INTO UserProfiles (FirstName,LastName,Avatar,AboutMySelf,User_Id) VALUES ('Lucija','Blazun','Avatar Lucija','About Lucija','2');");
            Sql("INSERT INTO UserProfiles (FirstName,LastName,Avatar,AboutMySelf,User_Id) VALUES ('Lovro','Blazun','Avatar Lovro','About Lovro','3');");


        }

        public override void Down()
        {
            // nothing
        }
    }
}
