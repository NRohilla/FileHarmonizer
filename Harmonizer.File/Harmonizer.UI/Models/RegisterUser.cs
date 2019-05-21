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
        public User User { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public AddressIinformation AddInfo { get; set; }
        public BPInfo BPinfo { get; set; }
    }

}