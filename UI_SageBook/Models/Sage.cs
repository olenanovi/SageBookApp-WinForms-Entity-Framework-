using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SageBook.Models
{
    public class Sage
    {
        [Key]
        public int IdSage { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Name { get; set; }

        public int Age { get; set; }

        public byte[] Image { get; set; }

        [MaxLength(150)]
        public string City { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public Sage()
        {
            Books = new HashSet<Book>();
        }
    }
}
