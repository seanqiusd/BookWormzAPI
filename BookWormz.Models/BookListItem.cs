using BookWormz.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class BookListItem
    {
        [Display(Name ="ISBN")]
        public string ISBN { get; set; }

        [Display(Name = "Title of Book")]
        public string BookTitle { get; set; }

        [Display(Name = "Author's First Name")]
        public string AuthorFirstName { get; set; }

        [Display(Name = "Author's Last Name")]
        public string AuthorLastName { get; set; }

        [Display(Name = "Genre")]
        public BookGenre GenreOfBook { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }


    }
}
