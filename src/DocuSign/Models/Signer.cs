using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSign.Models
{
    public class Signer
    {
        public string name { get; set; }
        public string email { get; set; }
        public string recipientId { get; set; }
        public string recipientIdGuid { get; set; }
        public string requireIdLookup { get; set; }
        public string userId { get; set; }
        public string clientUserId { get; set; }
        public string routingOrder { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public string templateLocked { get; set; }
        public string templateRequired { get; set; }
    }
}
