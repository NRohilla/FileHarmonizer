using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.UI.Models
{
    public class Individual
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public int Address { get; set; }
        public string ProjectName { get; set; }
        public string Email { get; set; }
        public string PrimaryInsuranceName { get; set; }
        public string PrimaryInsuranceNumber { get; set; }
        public string SecondaryInsuranceName { get; set; }
        public string SecondaryInsuranceNumber { get; set; }
        public int HomePage { get; set; }
        public string CompanyBPName { get; set; }
        public string CompanyBPAddress { get; set; }
        public string CompanyBPCity { get; set; }
        public string CompanyBPState { get; set; }
        public int CompanyBPZip { get; set; }
        public string CompanyBPWebsite { get; set; }
        public string CompanyBPPhone { get; set; }
       
    }
}