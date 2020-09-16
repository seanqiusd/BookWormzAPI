using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class CommentUpdate
    {
        [Required]
        [MaxLength(8000)]
        public string CommentText { get; set; }
    }
}
