using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class BookMaster
    {
        
        public int bookId { get; set; } //primary key
        public string BId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string BookImage { get; set; }
        public string BookDes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime BookAdded { get; set; }
        
    }
}
