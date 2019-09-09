namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginAdded1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "AccountCreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "AccountCreationDate", c => c.DateTime(nullable: false));
        }
    }
}
