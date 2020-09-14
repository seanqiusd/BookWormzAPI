﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Models
{
    public class ExchangeDetail
    {
        [Display(Name = "Exchange ID")]
        public int Id { get; set; }

        [Display(Name = "Book ID")]
        public string BookId { get; set; }

        [Display(Name = "Date Posted")]
        public DateTime Posted { get; set; }

        [Display(Name ="Sent Date")]
        public DateTime? SentDate { get; set; }

        public virtual ICollection<CommentDetail> Comments { get; set; } = new List<CommentDetail>();
    }
}
