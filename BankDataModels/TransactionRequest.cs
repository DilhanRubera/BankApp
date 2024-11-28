using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataModels
{
    public class TransactionRequest
    {

        public int SenderAccountNo { get; set; }
        public int ReceiverAccountNo { get; set; }

        public int TransactionAmount { get; set; }

        public string TransactionDescription { get; set; }


    }
}
