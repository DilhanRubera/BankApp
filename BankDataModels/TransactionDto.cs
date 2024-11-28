using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataModels
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public int SenderAccountNo { get; set; }
        public int ReceiverAccountNo { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
