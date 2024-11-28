using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankDataModels
{
    public class AdminUpdateRequest
    {
        public int adminId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int phoneNo { get; set; }
        public string address { get; set; }
        public string profilePic { get; set; }
    }
}
