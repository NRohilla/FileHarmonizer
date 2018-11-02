using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Harmonizer.UI.Models
{
    public class ForgotPassword
    {
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID must be entered")]
        public string UserID { get; set; }
        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Email ID must be entered")]
        [EmailAddress(ErrorMessage = "Email address isn't valid")]
        public string EmailID { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password must be entered")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).*$", ErrorMessage = "Password must contain at least one letter, one number and one special character(#?!@$%^&*-)")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm password must be entered")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmaPassword { get; set; }
    }
}