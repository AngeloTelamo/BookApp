using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookGenre { get; set; }
        public string BookAuthor { get; set; }
        public string BookSeries { get; set; }
        public string BookDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }

       // public int UserId { get; set; }
      //  public User Users { get; internal set; }
    }
}
