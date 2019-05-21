using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
   public  class PlanDetails
    {
        public int ID   { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FreeInfo { get; set; }
        public decimal Cost { get; set; }
        public int Validity { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int OrderDisplay { get; set; }
        public bool IsActive { get; set; }
        public string Group { get; set; }
        public int PlanFor { get; set; }// Weekly/Monthly/Yearly
    }
}
