using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using DocuSign.Models;
using DocuSign.Web.Models;
using DocuSign.Web.Utils;

namespace DocuSign.Web.Controllers
{
    public class SignatureTemplateController : DocuSignController
    {
        // POST api/signature
        public async Task<Envelope> Post([FromBody]SignatureRequestTemplate signatureRequestTemplate)
        {
            await CheckAuthInfo();
            var client = new DocuSignClient(BaseUrl, DocuSignCredentials);
            
            var envelope = await client.SendSignatureRequestAsync(
                signatureRequestTemplate.TemplateId,
                signatureRequestTemplate.RecipientName, 
                signatureRequestTemplate.RecipientEmail,
                signatureRequestTemplate.TemplateRole);

            return envelope;
        }

      
    }
}
