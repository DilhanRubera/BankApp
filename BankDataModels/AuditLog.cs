using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataModels
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string AdminUsername { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string AffectedResource { get; set; }
        public string Details { get; set; }
    }

}
