using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Data.Models
{
    public partial class BookMaster
    {
        [Key]
        public int BookId { get; set; } //primary key
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string BookImage { get; set; } //for image   
        public string BookSynopsis { get; set; }
        public string CreatedBy { get; set; }
        public DateTime BookAdded { get; set; }
        public string BookFile { get; set; }  //text file upload
        public int BookGenreId { get; set; } //foreign key for BookGenreMaster
        public BookGenreMaster genreMaster { get; set; }

        // Navigation property to represent the relationship
        public ICollection<Review> Reviews { get; set; } //
    }
}
