using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models.UserRatingModels
{
    class UserRatingCreate
    {
        [Required]
        [Display(Name = "Exchange Id Number")]
        public int ExchangeId { get; set; }

        [Required]
        [Display(Name = "User Id to be rated")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Exchange Rating score")]
        [Range(1,10)]
        public double ExchangeRating { get; set; }
    }
}
