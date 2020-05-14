using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core
{
    public class TagModel
    {
        public string FHnumber { get; set; }
        public string AssociateNumber { get; set; }
        public List<string> Tags { get; set; }
    }

    public class TagViewModel
    {
        public string FHnumber { get; set; }
        public string TagID { get; set; }
        public string ShareValue { get; set; }

    }
}
