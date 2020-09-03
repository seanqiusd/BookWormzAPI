using BookWormz.Data;
using BookWormz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Services
{
    public class ExchangeService
    {
        private readonly Guid _userId;

        public ExchangeService(Guid userId)
        {
            _userId = userId;
        }

        //public bool CreateExchange(ExchangeCreate model)
        //{
        //    var entity =
        //        new Exchange()
        //        {
        //            Id = model.Id,
        //            BookId = model.BookId,
        //            Posted = model.Posted,
        //            SentDate = model.SentDate
        //        };

        //    using (var ctx = new ApplicationDbContext())
        //    {
        //        //entity.
        //    }
        //}
    }
}
