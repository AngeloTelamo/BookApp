using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASI.Basecode.Services.ServiceModels.BookMasterListViewModel;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookGenreList
    {
        public List<BookGenres> GenreList { get; set; }
        public GenreListFilterModel Filters { get; set; }
        public class GenreListFilterModel
        {
            public int GenreId { get; set; }
            public string GenreName { get; set; }
            public string SearchTerm { get; set; }
        }
    }
}
