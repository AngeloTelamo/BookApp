using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Services.Annotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookMasterEditViewModel
    {
        [Required(ErrorMessage = "Book ID is required.")]
        public int BookId { get; set; } //bookId

        [Required(ErrorMessage = "Book Title is required.")]
        public string BookTitle { get; set; } 

        [Required(ErrorMessage = "Book Author is required.")]
        public string BookAuthor { get; set; }

        public string BookImage { get; set; }

       // [Required(ErrorMessage = "Book Des is required.")]
      //  public string BookGenre { get; set; }

        [Required(ErrorMessage = "Book Des is required.")]
        public string BookSynopsis { get; set; }
    }
}
