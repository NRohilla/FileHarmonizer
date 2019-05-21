using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Models
{
    public class ForgotPasswordModel
    {
        public string UserId { get; set; }
      
        public string Email { get; set; }
       
        public string Password { get; set; }
       
        public string ConfirmPassword { get; set; }
    }
}