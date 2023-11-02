using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookViewModel
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

        //public int UserId {  get; set; } //foreign key

        //public UserViewModel userViewModels { get; set; } //reference navigation property
             
      /// <summary>
      /// [Required(ErrorMessage = "Password is required.")]
      
        ///public string Password { get; set; }

      ///        [Required(ErrorMessage = "Confirmation Password is required.")]
      ///     [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
      ///  public string ConfirmPassword { get; set; }
        /// </summary>
    }
}
