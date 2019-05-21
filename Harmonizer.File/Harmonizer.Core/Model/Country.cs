using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
   public class Country
    {
        public int ID { get; set; }
        public string Alpha3 { get; set; }
        public string Alpha2 { get; set; }
        public string UNCode { get; set; }
        public string CountryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public string Operation { get; set; }

    }
}
