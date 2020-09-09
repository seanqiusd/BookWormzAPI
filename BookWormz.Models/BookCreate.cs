using BookWormz.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class BookCreate
    {
       
        [Required]
        public string ISBN { get; set; } 

        [Required]
        [MinLength(1, ErrorMessage ="At least 1 character")]
        public string BookTitle { get; set; }

       [Required]
        public string AuthorFirstName { get; set; }

       [Required]
        public string AuthorLastName { get; set; }

        [Required]
        public BookGenre GenreOfBook { get; set; }

       [Required]
       [MinLength(15, ErrorMessage ="At least 15 characters")]
        public string Description { get; set; }


    }
}
