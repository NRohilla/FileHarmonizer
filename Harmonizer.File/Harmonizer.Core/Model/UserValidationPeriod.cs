using System;
using System.Collections.Generic;
using System.Text;

namespace Harmonizer.Core.Model
{
   public  class UserValidationPeriod
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string UserType { get; set; }
    }
}
