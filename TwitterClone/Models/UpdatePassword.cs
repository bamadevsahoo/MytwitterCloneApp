using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Models
{
    public class UpdatePassword
    {


        [Display(Name = "Old password")]
        [Required(ErrorMessage = "Please enter old password")]
        [MaxLength(length: 100,ErrorMessage = "Maximum length 100 exceeded")]
        public string oldpassWord { get; set; }

        [Display(Name = "New password")]
        [Required(ErrorMessage = "Please enter new password")]
        [MaxLength(length: 100, ErrorMessage = "Maximum length 100 exceeded")]
        public string NewpassWord { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please confirm password")]
        [MaxLength(length: 100, ErrorMessage = "Maximum length 100 exceeded")]
        public string ConfirmpassWord { get; set; }
    }

}