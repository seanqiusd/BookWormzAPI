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
        [Required]
        public int CommentId { get; set; }

    }
}
