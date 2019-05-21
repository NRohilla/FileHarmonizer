using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class CHFilter
    {
        public int ID { get; set; }
        public string BPID { get; set; }
        public string SECID { get; set; }
        public string MyProperty { get; set; }
        public string INDID { get; set; }
        public string CATID { get; set; }
        public string FLTRSEC { get; set; }
        public string FLTRNUM { get; set; }
        public string FilterType { get; set; }
        public string FLTRID { get; set; }
        public string FilterDesc { get; set; }
        public string FilterText { get; set; }
        public string FilterName { get; set; }
        public string CFLTRID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FileID { get; set; }
        public string UserID {get;set;}
        public string SECCODE { get; set; }
        public string HFLTRID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Boolean IsDeleted { get; set; }
        public int AssignScheme { get; set; }
        public Boolean IsArchive { get; set; }

    }
}
