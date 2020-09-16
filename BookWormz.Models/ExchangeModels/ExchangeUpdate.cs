using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models.ExchangeModels
{
    public class ExchangeUpdate
    {
        public string BookId { get; set; }

        public DateTime? Posted { get; set; }

        public DateTime? SentDate { get; set; }

        public string ReceiverId { get; set; }
    }
}
