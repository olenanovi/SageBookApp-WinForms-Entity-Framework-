using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SageBook.Models
{
    public class Book
    {
        [Key]
        public int IdBook { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public virtual ICollection<Sage> Sages { get; set; }
        public Book()
        {
            Sages = new HashSet<Sage>();
        }
    }
}
