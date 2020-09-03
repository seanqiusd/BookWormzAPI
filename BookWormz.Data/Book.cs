using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Data
{
    public class Book
    {
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string BookTitle { get; set; }

        [Required]
        public string AuthorFirstName { get; set; }

        [Required]
        public string AuthorLastName { get; set; }
        public string AuthorFullName
        {
            get
            {
                return $"{AuthorFirstName} {AuthorLastName}";
            }
        }

        [Required]
        public BookGenre GenreOfBook { get; set; } //many eneums test

        [Required]
        public string Description { get; set; }

        public int NumberAvailable { get; set; }

    }
    public enum BookGenre {Fantasy, Adventure, Romance, Contemporary, Dystopian, Mystery, Horror, Thriller, Paranormal, HistoricalFiction, ScienceFiction, Memoir, Cooking, Art, SelfHelp, Development, Motivational, Health, History, Travel, Guide, FamiliesandRelationships, Humor, Children }
}
