using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.UI.Models
{
    public class Association
    {
        public string FHnumber { get; set; }
        public string Associate { get; set; }
        public bool AssocStatus { get; set; }
        public DateTime OriginalDateofAssoc { get; set; }
        public DateTime AssocCanceledDate { get; set; }
        public string AssocCanceledBy { get; set; }
    }
}