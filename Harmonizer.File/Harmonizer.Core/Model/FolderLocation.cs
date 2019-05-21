using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class FolderLocation
    {
        public int ID { get; set; }
        public string BPID { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Conversion { get; set; }
        public string Filter { get; set; }
        public string Archive { get; set; }
        public string Template { get; set; }
        public string Report { get; set; }
        public string Changelog { get; set; }
        public string Errorlog { get; set; }
    }
}
