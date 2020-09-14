using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Data
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Commenter))]
        public string CommenterId { get; set; }
        public virtual ApplicationUser Commenter { get; set; }


        [ForeignKey(nameof(Exchange))]
        public int ExchangeId { get; set; }
        public virtual Exchange Exchange { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    }
}
