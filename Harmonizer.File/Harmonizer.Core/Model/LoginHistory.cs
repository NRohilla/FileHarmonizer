using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class LoginHistory
    {
        public string UserID { get; set; }
        public string TerminalID { get; set; }
        public string Server { get; set; }
        public string TerminalIP { get; set; }
        public string LogonLang { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginTime { get; set; }
        public int NumberOfSignIn { get; set; }
    }
}
