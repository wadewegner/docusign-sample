using System.IO;

namespace DocuSign.Web.Models
{
    public class SignatureRequestDocument
    {
        public string DocumentName  { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string ContentType { get; set; }
        public FileStream DocumentStream { get; set; }
    }
}