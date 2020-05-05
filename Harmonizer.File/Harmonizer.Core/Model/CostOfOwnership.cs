using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
  public class CostOfOwnership
    {
        public int ID { get; set; }
        public string FHnumber { get; set; }
        public string Associate { get; set; }
        public DateTime DateProcessed { get; set; }
        public TimeSpan TimeProcessed { get; set; }
        public string TransactionName { get; set; }
        public string Msg { get; set; }
        public int TransactionCount { get; set; }
    }
}
