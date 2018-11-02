using System;
using System.Collections.Generic;
using System.Text;


namespace Harmonizer.Core.Model
{
   public  class User
    {
        
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string ConfiramPassword { get; set; }
        public string PasswordHash { get; set; }
        public string InitialPassword { get; set; }
        public string CodeVersion { get; set; }
        public string PasswordHasVAlue { get; set; }
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpire { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ActiveDate { get; set; }
        public int Role { get; set; }
    }
}
