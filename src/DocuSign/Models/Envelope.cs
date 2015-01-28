using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSign.Models
{
    public class Envelope
    {
        public string envelopeId { get; set; }
        public string uri { get; set; }
        public string statusDateTime { get; set; }
        public string status { get; set; }
        public string documentsUri { get; set; }
        public string recipientsUri { get; set; }
        public string envelopeUri { get; set; }
        public string emailSubject { get; set; }
        public string customFieldsUri { get; set; }
        public string notificationUri { get; set; }
        public string enableWetSign { get; set; }
        public string allowReassign { get; set; }
        public string createdDateTime { get; set; }
        public string lastModifiedDateTime { get; set; }
        public string sentDateTime { get; set; }
        public string statusChangedDateTime { get; set; }
        public string documentsCombinedUri { get; set; }
        public string certificateUri { get; set; }
        public string templatesUri { get; set; }
        public string purgeState { get; set; }
    }
}
