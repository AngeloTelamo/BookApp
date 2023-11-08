using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class BookMasterRepository : BaseRepository, IBookMasterRepository

    {
        public BookMasterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public IQueryable<BookMaster> GetBooks()
        {
            return this.GetDbSet<BookMaster>();
        }

        public void AddBook(BookMaster book)
        {
            this.GetDbSet<BookMaster>().Add(book);
            UnitOfWork.SaveChanges();
        }

        

        public bool BookExists(string bId)
        {
            return this.GetDbSet<BookMaster>().Any(x => x.BId == bId);
        }
    }
}
