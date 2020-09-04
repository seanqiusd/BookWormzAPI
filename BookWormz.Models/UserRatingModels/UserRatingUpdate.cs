using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models.UserRatingModels
{
    class UserRatingUpdate
    {
        [Required]
        [Display(Name = "Exchange Rating score")]
        [Range(1, 10)]
        public double ExchangeRating { get; set; }

        [Required]
        public int ExchangeId { get; set; }
    }
}
