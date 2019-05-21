using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class LanguageTimeZone
    {
        public int ID { get; set; }
        public string Language { get; set; }
        public string LanguageDescription { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
