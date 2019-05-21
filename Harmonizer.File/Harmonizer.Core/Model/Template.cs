using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class Template
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string BPID { get; set; }
        public string HFLTRID { get; set; }
        public string TEMPNUM { get; set; }
        public string TEMPID { get; set; }
        public string Partner { get; set; }
        public string TemplateType { get; set; }
        public string TemplateDesc { get; set; }
        public string TemplateName { get; set; }
        public string DocExt { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int FileID { get; set; }

        public string SECID { get; set; }
        public string SECCODE { get; set; }
        public string FilterType { get; set; }
        public string TemplateText {get;set;}
        public Boolean IsDeleted { get; set; }
        public Boolean IsArchive { get; set; }
        public string InternalExternal { get; set; }
    }
}
