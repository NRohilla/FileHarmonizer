using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Models
{
    public class FeedbackModel
    {
        public string Name { get; set; }
     
        public string Email { get; set; }
     
        public string ContactNo { get; set; }
       
        public string Subject { get; set; }
        
        public string Brief { get; set; }
    }
}