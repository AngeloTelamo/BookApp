using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookRepository
    {
        IQueryable<Book> GetBooks();
        bool BookExists(string bookId);
        void AddBooks(Book book);
        void UpdateBooks(Book book);
        void DeleteBooks(Book book);
    }
}
