namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnswersSEED : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Answers (Body,PostId,UserId) VALUES ('OdgovorNaPost1','1','1');");
            Sql("INSERT INTO Answers (Body,PostId,UserId) VALUES ('OdgovorNaPost2','2','2');");

            //Sql("GO;");

            //Sql("UPDATE Posts SET  (Theme_Id,AnswerId) VALUES ('1','1') WHERE Posts.Id='1';");
            //Sql("UPDATE Posts SET  (Theme_Id,AnswerId) VALUES ('2','1') WHERE Posts.Id='2';");

        }

        public override void Down()
        {
        }
    }
}
