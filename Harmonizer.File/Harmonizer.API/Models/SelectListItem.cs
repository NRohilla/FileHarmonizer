using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Models
{
    public class SelectListItem
    {
      
        public bool Selected { get; set; }
        
        public string Text { get; set; }
      
        public string Value { get; set; }
    }
}