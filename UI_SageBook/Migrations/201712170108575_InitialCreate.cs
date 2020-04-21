namespace SageBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        IdBook = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 1000),
                        Description = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.IdBook);
            
            CreateTable(
                "dbo.Sages",
                c => new
                    {
                        IdSage = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 1000),
                        Age = c.Int(nullable: false),
                        Image = c.Binary(),
                        City = c.String(),
                    })
                .PrimaryKey(t => t.IdSage);
            
            CreateTable(
                "dbo.SageBooks",
                c => new
                    {
                        Sage_IdSage = c.Int(nullable: false),
                        Book_IdBook = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Sage_IdSage, t.Book_IdBook })
                .ForeignKey("dbo.Sages", t => t.Sage_IdSage, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_IdBook, cascadeDelete: true)
                .Index(t => t.Sage_IdSage)
                .Index(t => t.Book_IdBook);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SageBooks", "Book_IdBook", "dbo.Books");
            DropForeignKey("dbo.SageBooks", "Sage_IdSage", "dbo.Sages");
            DropIndex("dbo.SageBooks", new[] { "Book_IdBook" });
            DropIndex("dbo.SageBooks", new[] { "Sage_IdSage" });
            DropTable("dbo.SageBooks");
            DropTable("dbo.Sages");
            DropTable("dbo.Books");
        }
    }
}
