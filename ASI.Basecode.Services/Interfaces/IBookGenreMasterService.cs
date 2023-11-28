using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IBookGenreMasterService
    {
        void AddGenre(BookGenres genre);
        void DeleteGenre(int genreId);
        BookGenres getGenreById(int genreId);
        BookGenreList GetGenreList(BookGenreList model);
    }
}
