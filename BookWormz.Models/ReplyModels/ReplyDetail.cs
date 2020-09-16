using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class ReplyDetail
    {
        [Display(Name ="Id")]
        public int Id { get; set; }

        [Display(Name ="Reply")]
        public string Text { get; set; }

        [Display(Name ="Commentor's Name")]
        public string CommentorsName { get; set; }

        public List<ReplyDetail> Replies { get; set; } = new List<ReplyDetail>();
    }
}
