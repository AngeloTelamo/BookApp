using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Services.Annotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

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
