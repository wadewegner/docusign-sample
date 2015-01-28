using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSign.Models
{
    public class Recipient
    {
        public List<Signer> signers { get; set; }
        // TODO: define these types in the future
        public List<object> agents { get; set; }
        public List<object> editors { get; set; }
        public List<object> intermediaries { get; set; }
        public List<object> carbonCopies { get; set; }
        public List<object> certifiedDeliveries { get; set; }
        public List<object> inPersonSigners { get; set; }
        public string recipientCount { get; set; }
        public string currentRoutingOrder { get; set; }
    }
}
