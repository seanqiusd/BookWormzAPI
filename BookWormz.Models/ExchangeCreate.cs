using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class ExchangeCreate
    {
        public string BookId { get; set; }

        public DateTime Posted { get; set; }

        public DateTime? SentDate { get; set; }



    }
}
