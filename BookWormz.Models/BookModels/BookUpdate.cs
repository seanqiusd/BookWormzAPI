using BookWormz.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class BookUpdate
    {
        [MinLength(1, ErrorMessage = "At least 1 character")]
        public string BookTitle { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public BookGenre? GenreOfBook { get; set; }

        [MinLength(15, ErrorMessage = "At least 15 characters")]
        public string Description { get; set; }
    }
}
