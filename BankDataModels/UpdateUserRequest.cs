using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankDataModels
{
    public class UpdateUserRequest
    {

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public int PhoneNo { get; set; }

        public string Password { get; set; }


        public string ProfilePicBase64 { get; set; }
    }
}
