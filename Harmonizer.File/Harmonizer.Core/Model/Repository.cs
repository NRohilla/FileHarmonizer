using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
   public  class Repository
    {
        public int ID { get; set; }
        public int TemplateID { get; set; }
        public string BPID { get; set; }
        public string UserID { get; set; }
        public string UTAGID { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string Org { get; set; }
        public string GlobPri { get; set; }
        public string Share { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string SearchWord { get; set; }
        public string Instruction { get; set; } 
    }
}
