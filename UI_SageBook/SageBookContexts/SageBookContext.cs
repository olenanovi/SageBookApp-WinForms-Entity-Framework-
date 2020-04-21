namespace SageBook.SageBookContexts
{

    using System.Data.Entity;
    using SageBook.Models;

    public class SageBookContext : DbContext
    {
        public SageBookContext()
            : base("name=SageBookContext")
        {
        }
        public DbSet<Book> Books { get; set; }

        public DbSet<Sage> Sages { get; set; }

    }
    

}