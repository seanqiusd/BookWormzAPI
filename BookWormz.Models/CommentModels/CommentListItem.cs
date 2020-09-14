using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class CommentListItem
    {
        public int Id { get; set; }
        [Display(Name = "Exchange Id")]
        public int ExchangeId { get; set; }

        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Commenter")]
        public string CommenterName { get; set; }

        [Display(Name = "Number Of Replies")]
        public int NumberOfReplies { get; set; }


    }
}
