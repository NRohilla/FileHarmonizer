using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class Tag
    {

        public int ID { get; set; }
        public int UTAGID { get; set; }
        public string UserID { get; set; }
        public string TagName { get; set; }
        public string Orig { get; set; }
        public string GlobPri { get; set; }
        public string Description { get; set; }
        public string Share { get; set; }
        public string SearchWord { get; set; }
        public string Instruction { get; set; }
        public string BPID { get; set; }
    }
}
