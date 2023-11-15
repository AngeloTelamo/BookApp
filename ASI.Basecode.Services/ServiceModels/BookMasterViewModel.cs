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
        public string BId { get; set; } //bookId

        [Required(ErrorMessage = "Book Title is required.")]
        public string BookTitle { get; set; } //

        [Required(ErrorMessage = "Book Author is required.")]
        public string BookAuthor { get; set; }

        public string BookDes { get; set; }
        
        public string BookImage { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile BookImageFile { get; set; }
    }
}
