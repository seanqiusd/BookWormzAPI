using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models.UserRatingModels
{
    public class UserRatingDetail
    {
        [Display(Name = "Exchange Id Number")]
        public int ExchangeId { get; set; }

        [Display(Name = "User Id to be rated")]
        public string UserId { get; set; }

        [Display(Name = "Exchange Rating score")]
        public double ExchangeRating { get; set; }
    }
}
