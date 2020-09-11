using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class ReplyCreate : CommentCreate
    {
        // may need to add ExchangeId here
        [Required]
        public int CommentId { get; set; }

    }
}
