using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASI.Basecode.Services.ServiceModels.ReviewList;

namespace ASI.Basecode.Services.ServiceModels
{
    public class ReviewList
    {
        public List<BookReviewsModel> ReviewLists { get; set; }
        public ReviewListFilterModel Filters { get; set; }
        public int TotalReviews { get; set; }

        public class ReviewListFilterModel
        {
            public string ReviewName { get; set; }
            public int BookIds { get; set; }
            public string SearchTerm { get; set; }
        }
    }
}
