using System;
using System.Collections.Generic;
using System.Text;


namespace Harmonizer.Core.Model
{
   public  class User
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }// only for layout- do not make as binary
        public string ConfirmPassword { get; set; }// only for layout
        public string PasswordHash { get; set; } // binary
        public string InitialPassword { get; set; } // binary
        public string CodeVersion { get; set; }
        public string PasswordHasValue { get; set; } // binary
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpire { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime ActiveDate { get; set; }
        public int Role { get; set; }
        public Byte[] OriganalPass { get; set; }
    }
}
