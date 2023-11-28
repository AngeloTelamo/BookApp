using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class BookGenreMasterRepository : BaseRepository, IBookGenreMasterRepository
    {
        public BookGenreMasterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public void AddGenre(BookGenreMaster genreMaster)
        {
            this.GetDbSet<BookGenreMaster>().Add(genreMaster);  
            UnitOfWork.SaveChanges();
        }

        public void DeleteGenres(BookGenreMaster deleteGenre)
        {
            this.GetDbSet<BookGenreMaster>().Remove(deleteGenre);
            UnitOfWork.SaveChanges();
        }

        public IQueryable<BookGenreMaster> GetGenre()
        {
            return this.GetDbSet<BookGenreMaster>();
        }

        public BookGenreMaster getGenreById(int genreId)
        {
            return this.GetDbSet<BookGenreMaster>()
            .Include(b => b.Books)
            .FirstOrDefault(x => x.GenreId == genreId);
        }
    }
}
