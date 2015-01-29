namespace DocuSign.Web.Models
{
    public class SignatureRequestTemplate
    {
        public string TemplateId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string TemplateRole { get; set; }
    }
}