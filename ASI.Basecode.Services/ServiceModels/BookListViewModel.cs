using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookListViewModel
    {
        public List<BookViewModel> BookList { get; set; }
        public BookListFilterModel Filters { get; set; }

        public class BookListFilterModel
        {
            public string BookId { get; set; }

            [DisplayName("Book Title")]
            public string BookTitle { get; set; }


            [DisplayName("Genre")]
            public string BookGenre { get; set; }

            [DisplayName("Author")]
            public string BookAuthor { get; set; }

            [DisplayName("Series")]
            public string BookSeries { get; set; }

        }
    }
}
