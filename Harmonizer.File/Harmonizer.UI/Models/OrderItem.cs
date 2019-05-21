using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.UI.Models
{
    public class OrderItem
    {
        public string sku { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public string tax { get; set; }
        public string url { get; set; }
        public string category { get; set; }
    }
}