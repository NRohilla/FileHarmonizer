using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class HarmonizeTemplate
    {
       
        public int ID { get; set; }
        public int TemplateID { get; set; }
        public string BPID { get; set; }
        public string UserID { get; set; }
        public string TemplateName { get; set; }
        public string TemplatePath { get; set; }
        public int NoOfDownloads { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Comment { get; set; }
        public string HTFHNumber { get; set; }
        public Boolean IsArchive { get; set; }
    }
}
