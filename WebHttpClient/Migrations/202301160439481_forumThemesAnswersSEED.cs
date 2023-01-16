namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forumThemesAnswersSEED : DbMigration
    {
        public override void Up()
        {
            //Sql("INSERT INTO UserStatus (StatusTitle,UStatus,UserId) VALUES ('User','3','3');");

            Sql("INSERT INTO Fora (Name,ThemesCounter,UserCounter) VALUES ('PrviForum','0','3');");


            Sql("INSERT INTO Themes (Title,Value,UserId,ForumId) VALUES ('Monitori','0','1','1');");
            Sql("INSERT INTO Themes (Title,Value,UserId,ForumId) VALUES ('Tipkovnice','0','2','1');");
            Sql("INSERT INTO Themes (Title,Value,UserId,ForumId) VALUES ('Laptopi','0','3','1');");

            Sql("INSERT INTO Answers (Body,PostId,UserId) VALUES ('OdgovorNaPost1','1','1');");
            Sql("INSERT INTO Answers (Body,PostId,UserId) VALUES ('OdgovorNaPost2','2','2');");



            Sql("UPDATE Posts SET  (Theme_Id,AnswerId) VALUES ('1','1') WHERE Posts.Id='1';");
            Sql("UPDATE Posts SET  (Theme_Id,AnswerId) VALUES ('2','1') WHERE Posts.Id='2';");


        }

        public override void Down()
        {
        }
    }
}
