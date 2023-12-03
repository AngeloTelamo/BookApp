using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TopBooks
    {
        public List<BookMasterViewModel> TopList { get; set; }
        public TopListFilterModel BookFilter { get; set; }

        public class TopListFilterModel
        {
            public int BookId { get; set; }

            [DisplayName("Book Title")]
            public string BookTitle { get; set; }
            [DisplayName("Book Author")]
            public string BookAuthor { get; set; }
            public string SearchTerm { get; set; }
            public string GenreName { get; set; }
            public int GenreId { get; set; }
            public int ReviewId { get; set; }
            public int ReviewCount { get; set; }
            public string Reviewname { get; set; }
            public string ReviewsComments { get; set; }
        }
    }
}
