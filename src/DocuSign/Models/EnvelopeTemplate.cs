namespace DocuSign.Models
{
    public class EnvelopeTemplate
    {
        public string templateId { get; set; }
        public string name { get; set; }
        public string shared { get; set; }
        public string password { get; set; }
        public string description { get; set; }
        public string lastModified { get; set; }
        public int pageCount { get; set; }
        public string uri { get; set; }
        public string folderName { get; set; }
        public string folderId { get; set; }
        public string folderUri { get; set; }
        public Owner owner { get; set; }
        public string emailSubject { get; set; }
        public string emailBlurb { get; set; }
        public string signingLocation { get; set; }
        public string authoritativeCopy { get; set; }
        public string enableWetSign { get; set; }
        public string allowMarkup { get; set; }
    }
}
