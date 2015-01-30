using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSign.Models
{
    public class Templates
    {
        public List<EnvelopeTemplate> envelopeTemplates { get; set; }
        public string resultSetSize { get; set; }
        public string startPosition { get; set; }
        public string endPosition { get; set; }
        public string totalSetSize { get; set; }
    }
}
