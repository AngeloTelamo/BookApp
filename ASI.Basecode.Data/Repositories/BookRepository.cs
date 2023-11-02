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
    public class BookRepository : BaseRepository, IBookRepository
    {
        public BookRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {

        }
        public IQueryable<Book> GetBooks()
        {
            return this.GetDbSet<Book>();
        }

        public bool BookExists(string bookId)
        {
            return this.GetDbSet<Book>().Any(x => x.BookId == bookId);
           
        }

        public void AddBooks(Book book)
        {
            this.GetDbSet<Book>().Add(book);
            UnitOfWork.SaveChanges();
          
        }

        public void UpdateBooks(Book book)
        {
            this.GetDbSet<Book>().Update(book);
            UnitOfWork.SaveChanges();
          
        }

        public void DeleteBooks(Book book)
        {
            this.GetDbSet<Book>().Remove(book);
            UnitOfWork.SaveChanges();
           
        }
    }
}
