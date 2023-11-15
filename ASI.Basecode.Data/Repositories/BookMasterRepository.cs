using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data.Repositories
{
    public class BookMasterRepository : BaseRepository, IBookMasterRepository
    {
        public BookMasterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public bool BookExists(string bId)
        {
            return this.GetDbSet<BookMaster>().Any(x => x.BId == bId);
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
    }
}


