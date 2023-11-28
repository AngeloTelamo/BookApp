using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data.Repositories
{
    public class BookMasterRepository : BaseRepository, IBookMasterRepository
    {
        public BookMasterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public bool BookExists(int bookId)
        {
            return this.GetDbSet<BookMaster>().Any(x => x.BookId == bookId);
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
        public void UpdateBooks(BookMaster book)
        {
            this.GetDbSet<BookMaster>().Update(book);
            UnitOfWork.SaveChanges();
        }
        public void DeleteBooks(BookMaster book)  //deleting the components of book
        {
           this.GetDbSet<BookMaster>().Remove(book);
            UnitOfWork.SaveChanges();
        }

        public BookMaster GetBookById(int bookId)
        {
            return this.GetDbSet<BookMaster>()
                .Include(b => b.Reviews)
                .FirstOrDefault(x => x.BookId == bookId);
        }
    }
}


