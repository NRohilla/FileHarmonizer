using System;
using System.Collections.Generic;
using System.Text;

namespace Harmonizer.Core.Model
{
    public class PersonalInformation
    {
        public int PersonalID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; }
        public int UserRole { get; set; }

        public string Name2 { get; set; }
        public string MiddleName { get; set; }

        public string AKA { get; set; }//Also Known AS (Petname)

        public string Initials { get; set; }// First Value of name

        public string Country { get; set; }
        public string Profession { get; set; }
        public string Gender { get; set; }

        public string Language { get; set; }
        public string Phone { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string MiddelInitials { get; set; }
        public DateTime? DateOfBorn { get; set; }
        public string BirthCountry { get; set; }

        public string DefaulCountry { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string WorkPhone { get; set; }
        public string TeamMemberRole { get; set; }
    }
}
