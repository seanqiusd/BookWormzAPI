using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class ExchangeSmListItem
    {
        [Display(Name = "Exchange ID")]
        public int Id { get; set; }

        [Display(Name = "Date Posted")]
        public DateTime Posted { get; set; }

        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }

        [Display(Name = "Is available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Sender Rating")]
        public double? SenderRating { get; set; }
    }
}
