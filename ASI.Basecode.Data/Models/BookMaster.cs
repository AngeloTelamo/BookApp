using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class BookMaster
    {
        
        public int BookId { get; set; } //primary key
        public string BId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string BookImage { get; set; } //for image
        public string BookDes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime BookAdded { get; set; }
    }
}
