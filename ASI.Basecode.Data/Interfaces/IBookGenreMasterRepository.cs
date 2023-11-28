using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookGenreMasterRepository
    {
        void AddGenre(BookGenreMaster genreMaster);
        void DeleteGenres(BookGenreMaster deleteGenre);
        IQueryable<BookGenreMaster> GetGenre();
        BookGenreMaster getGenreById(int genreId);
    }
}
