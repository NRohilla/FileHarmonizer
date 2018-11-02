using System;
using System.Collections.Generic;
using System.Text;

namespace Harmonizer.Core.Model
{
    public class UserRole
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string UserType { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
