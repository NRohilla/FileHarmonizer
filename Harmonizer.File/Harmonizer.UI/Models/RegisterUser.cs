using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Harmonizer.Core.Model;


namespace Harmonizer.UI.Models
{
    public class RegisterUser
    {
        //public string UserID { get; set; }
        //public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
        //public string EmailID { get; set; }

        //public string Title { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
       
        //public string UserRole { get; set; }

        //public string Name2 { get; set; }
        //public string MiddleName { get; set; }

        //public string AKA { get; set; }

        //public string Initials { get; set; }

        //public string Country { get; set; }
        //public string Profession { get; set; }
        //public string Gender { get; set; }

        //public string Language { get; set; }

        public User User { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public AddressIinformation AddInfo { get; set; }

    }

}