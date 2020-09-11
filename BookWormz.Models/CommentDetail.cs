using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class CommentDetail
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name ="Comment")]
        public string Text { get; set; }

        public virtual ICollection<ReplyDetail> Replies { get; set; } = new List<ReplyDetail>();
    }
}
