using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
   public class PaymentModel
    {
        public string cart { get; set; }
        public DateTime create_time { get; set; }
        public string id { get; set; }// PAYID-.....
        public string state { get; set; }// approved
        public string invoice { get; set; }//invoice_number
        public string description { get; set; }
        public string amount { get; set; }// transactions.amount.total
        public string currency { get; set; } //transactions.amount.currency
        public string paymentmethod { get; set; } // payer.payment_methods
        public string failuarreasion { get; set; }
        public string failuerexception { get; set; }
        public string payerId { get; set; }
        public string TransactionId { get; set; }
        public string PlanId { get; set; }
        public string UserId { get; set; }
        public string BPID { get; set; }
        public string TokenId { get; set; }
    }
}
