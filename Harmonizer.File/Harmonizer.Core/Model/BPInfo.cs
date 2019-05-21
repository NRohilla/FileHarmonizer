using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class BPInfo
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int AddressID { get; set; }
        public string BPType { get; set; }
        public string Partner { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string MainPhone { get; set; }
        public string SecondPhone { get; set; }
        public string TollFreePhone { get; set; }
        public string Fax { get; set; }
        public string OneTimeAcct { get; set; }
        public string Zip { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? CreatedBy { get; set; }
        public DateTime? ChangeOnDate { get; set; }
        public DateTime? CompanyDate { get; set; }

        public string AccountGroup { get; set; }
        public string Language { get; set; }
        public string TaxJurisdication { get; set; }
        public string TaxNumber { get; set; }
        public string Website { get; set; }
        public string ChangedBy { get; set; }
        public string ChangedTime { get; set; }
        public string CompName { get; set; }
        public string CompName2 { get; set; }
        public string Department { get; set; }
        
        public string Email { get; set; }
        public string ContactNameFirst { get; set; }
        public string ContactNameLast { get; set; }
        public string CompanyEIN { get; set; }
        
        //public string TollFreeNo { get; set; }
        //public string LastName { get; set; }
    }
}
