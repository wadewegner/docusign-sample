using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSign.Models
{
    public class Envelopes
    {
        public string resultSetSize { get; set; }
        public string totalSetSize { get; set; }
        public string startPosition { get; set; }
        public string endPosition { get; set; }
        public string nextUri { get; set; }
        public string previousUri { get; set; }
        public List<Envelope> envelopes { get; set; }
    }
}
