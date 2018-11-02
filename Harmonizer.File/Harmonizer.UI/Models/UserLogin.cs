using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Harmonizer.UI.Models
{
    public class UserLogin
    {
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID must be entered")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Password must be entered")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}