using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class Association
    {
        public string FHnumber { get; set; }
        public string Associate { get; set; }
        public string AssocCanceledBy { get; set; }
        public bool AssocStatus { get; set; }
        public DateTime OriginalDateofAssoc { get; set; }
        public DateTime AssocCanceledDate { get; set; }
    }
}
