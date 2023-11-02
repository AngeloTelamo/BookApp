using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Services.Annotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookEditViewModel
    {
        [Required(ErrorMessage = "BookId is required.")]
        public string BookId { get; set; }

        [Required(ErrorMessage = "Book Title is required.")]
        public string BookTitle { get; set; }

        [Required(ErrorMessage = "Book Genre is required.")]
        public string BookGenre { get; set; }

        [Required(ErrorMessage = "Book Author is required.")]
        public string BookAuthor { get; set; }

        [Required(ErrorMessage = "Book Series is required.")]
        public string BookSeries { get; set; }

        [Required(ErrorMessage = "Book Description is required.")]
        public string BookDescription { get; set; }

        [CustomRequired]
        [Display(Name = "Update Password")]
        public bool UpdatePasswordFlg { get; set; }

        [ConditionalRequired("UpdatePasswordFlg")]
        public string Password { get; set; }

        [ConditionalRequired("UpdatePasswordFlg")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        
    }
}
