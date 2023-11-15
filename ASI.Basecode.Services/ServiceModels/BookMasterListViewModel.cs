﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookMasterListViewModel
    {
        public List<BookMasterViewModel> BookList { get; set; }
        public BookListFilterModel Filters { get; set; }

        public class BookListFilterModel
        {
            public string BId { get; set; }

            [DisplayName("Book Title")]
            public string BookTitle { get; set; }

            [DisplayName("Book Author")]
            public string BookAuthor { get; set; }

            [DisplayName("Book Description")]
            public string BookDes { get; set; }

            public string BookImageUrl { get; set; } // Add this property

        }
    }
}
