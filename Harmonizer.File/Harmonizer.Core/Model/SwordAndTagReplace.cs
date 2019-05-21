using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
   public  class SwordAndTagReplace
    {
        public int ID { get; set; }
        public string SearchWord { get; set; }
        public string TagName { get; set; }
        public string Instruction { get; set; }
        public int FileID { get; set; }
        public string FilterType { get; set; }
    }
}
