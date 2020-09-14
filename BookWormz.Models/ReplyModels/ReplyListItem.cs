using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class ReplyListItem
    {
        public int ExchangeId { get; set; }

        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name ="Replier")]
        public string RepliersName { get; set; }

    }
}
