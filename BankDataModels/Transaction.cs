using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankDataModels
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public string TransactionDescription { get; set; }

        public int TransactionAmount { get; set; }

        [JsonIgnore]
        public Account SenderAccount { get; set; }

        public string SenderName { get; set;}
        public int SenderAccountNo { get; set; }

        public int SenderId { get; set; }


        public string ReceiverName { get; set;}

        public int ReceiverAccountNo { get; set; }

        public int ReceiverId { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
