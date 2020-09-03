using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Data
{
    public class Book
    {
        public string ISBN { get; set; }
        public string BookTitle { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorFullName
        {
            get
            {
                return $"{AuthorFirstName} {AuthorLastName}";
            }
        }
        public BookGenre GenreOfBook { get; set; } //many eneums test
        public string Description { get; set; }
        public int NumberAvaiable { get; set; }

    }
    public enum BookGenre {Fantasy, Adventure, Romance, Contemporary, Dystopian, Mystery, Horror, Thriller, Paranormal, HistoricalFiction, ScienceFiction, Memoir, Cooking, Art, SelfHelp, Development, Motivational, Health, History, Travel, Guide, FamiliesandRelationships, Humor, Children }
}
