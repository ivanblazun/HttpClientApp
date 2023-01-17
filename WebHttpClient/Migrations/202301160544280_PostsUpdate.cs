namespace WebHttpClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostsUpdate : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Posts SET  Theme_Id='9',AnswerId='1' WHERE Posts.Id='1';");
            Sql("UPDATE Posts SET  Theme_Id='10',AnswerId='2' WHERE Posts.Id='2';");

        }

        public override void Down()
        {
        }
    }
}
