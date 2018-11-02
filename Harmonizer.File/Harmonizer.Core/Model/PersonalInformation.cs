using System;
using System.Collections.Generic;
using System.Text;

namespace Harmonizer.Core.Model
{
    public class PersonalInformation
    {
        public int PersonalID { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string EmailID { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserRole { get; set; }

        public string Name2 { get; set; }
        public string MiddleName { get; set; }

        public string AKA { get; set; }

        public string Initials { get; set; }

        public string Country { get; set; }
        public string Profession { get; set; }
        public string Gender { get; set; }

        public string Language { get; set; }
    }
}
