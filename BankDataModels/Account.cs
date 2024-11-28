using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankDataModels
{
    public class Account
    {

        public int AccountId { get; set; }

        public int AccountNumber { get; set; }
        public string AccountName { get; set; }

        public int AccountBalance { get; set; }
        public int AccountHolderId { get; set; }

        [JsonIgnore]
        public User AccountHolder { get; set; }

        [JsonIgnore]
        public List<Transaction> TransactionList { get; set; }

        public Boolean isActivated { get; set; }

    }
}
