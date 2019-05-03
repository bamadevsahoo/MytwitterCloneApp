using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Models
{
    public class Account
    {
        [Display(Name ="Username")]
        [Required(ErrorMessage ="Please enter user name")]
        [MaxLength(length:25, ErrorMessage = "Maximum length 25 exceeded")]
        public string userName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter password")]
        [MaxLength(length: 100, ErrorMessage = "Maximum length 100 exceeded")]
        public string passWord { get; set; }
    }
}