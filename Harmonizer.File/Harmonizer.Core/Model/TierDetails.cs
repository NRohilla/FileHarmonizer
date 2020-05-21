using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
    public class TierDetails
    {
        public int ID { get; set; }
        public int UserCount { get; set; }
        public int Tier { get; set; }
        public string PartnerType { get; set; }
        public string BusinessPartners { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal AnnualCost { get; set; }
        public decimal PerUserCost { get; set; }
        public bool UserCareFlag { get; set; }
        public decimal UserCareValue { get; set; }
        public decimal PerUserCostwithCareValue { get; set; }
        public decimal MonthlyCostWithCare { get; set; }
    }
}
