using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public  class Sector
    {
        public int ID { get; set; }
        public string SECID { get; set; }
        public string SECCode { get; set; }
        public string Description { get; set; }
        public string TAGSEC { get; set; }
        public int ActionId { get; set; }

    }
}
