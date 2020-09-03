using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Data
{
    public class UserReview
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Reviewer))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser Reviewer { get; set; }

        [ForeignKey(nameof(Exchange))]
        public int ExchangeId { get; set; }
        public virtual Exchange Exchange { get; set; }

        [Required]
        public double ExchangeRating { get; set; }
    }
}
