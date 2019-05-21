using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
   public  class UserScheme
    {
        public int ID { get; set; }
        public string FHNumber { get; set; }
        public int SchemeNum { get; set; }
        public string SchemeName { get; set; }
        public string SchemeDescription { get; set; }
        public string SchemeType { get; set; }
        public long Client { get; set; }
        public string Name { get; set; }
        public string RegistrationType { get; set; }
        public string ProjectType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string SchemeComment { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }

        public Boolean IsActive { get; set; }
        public Boolean IsArchive { get; set; }
        public Boolean IsDeleted { get; set; }
        public string FilterName { get; set; }
        public string TemplateName { get; set; }
        public string HarmonizeName { get; set; }
        public string Sector { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BPID { get; set; }
        public string Suggestion { get; set; }
    }
}
