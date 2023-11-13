using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookMasterRepository
    {
        IQueryable<BookMaster> GetBooks();
        bool BookExists(string bId);
        void AddBook(BookMaster book);
        void UpdateBooks(BookMaster book);
        void DeleteBooks(BookMaster book);
    }
}
