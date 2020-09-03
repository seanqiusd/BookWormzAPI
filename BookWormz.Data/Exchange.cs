using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Data
{
    public class Exchange
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(SenderId))]
        public string BookId { get; set; }
        public virtual ApplicationUser SenderId {get; set;}


        public bool IsAvailable { get; set; }

        public DateTime Posted { get; set; }

        public DateTime? SentDate { get; set; }


        [ForeignKey(nameof(ReceiverUser))]
        public int? ReceiverId { get; set; }
        public virtual ApplicationUser ReceiverUser { get; set; }

    }
}
