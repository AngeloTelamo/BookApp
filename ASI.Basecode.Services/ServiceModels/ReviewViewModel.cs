using System;
using System.ComponentModel.DataAnnotations;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.ServiceModels
{
    public class ReviewViewModel
    {
       // public int ReviewId { get; set; }

        [Required(ErrorMessage = "Review Name is required.")]
        public string ReviewName { get; set; }
        public int ReviewRatings { get; set; }
        [Required(ErrorMessage = "Review Comments are required.")]
        public string ReviewComments { get; set; }
      
    }
}
