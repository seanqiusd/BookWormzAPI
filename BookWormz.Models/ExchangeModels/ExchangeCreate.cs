using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class ExchangeCreate
    {
        [Required]
        public string BookId { get; set; }
        
        public DateTime? SentDate { get; set; }

        public string ReceiverUser { get; set; }
    }
}