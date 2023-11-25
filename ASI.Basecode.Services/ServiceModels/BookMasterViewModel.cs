using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookMasterViewModel
    {

        [Required(ErrorMessage = "Book ID is required.")]
        public int BookId { get; set; } 

        [Required(ErrorMessage = "Book Title is required.")]
        public string BookTitle { get; set; } 

        [Required(ErrorMessage = "Book Author is required.")]
        public string BookAuthor { get; set; }

        public string BookGenre { get; set; }

        [Required(ErrorMessage = "Book Summary is required.")]
        public string BookSynopsis { get; set; }

        public string BookImage { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile BookImageFile { get; set; } //method
    }
}
