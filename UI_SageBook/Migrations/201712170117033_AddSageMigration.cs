namespace SageBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSageMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sages", "City", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sages", "City", c => c.String());
        }
    }
}
