using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using DocuSign.Models;
using DocuSign.Web.Models;

namespace DocuSign.Web.Controllers
{
    public class SignatureController : ApiController
    {
        private static readonly string Username = ConfigurationManager.AppSettings["Username"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];
        private static readonly string IntegratorKey = ConfigurationManager.AppSettings["IntegratorKey"];

        private static string _baseUrl;
        private static string _docuSignCredentials;
        private static AuthenticationClient _auth;


        // POST api/signature
        public async Task<Envelope> Post([FromBody]SignatureRequestTemplate signatureRequestTemplate)
        {
            await CheckAuthInfo();

            var client = new DocuSignClient(_baseUrl, _docuSignCredentials);
            
            var envelope = await client.SendSignatureRequestAsync(
                signatureRequestTemplate.TemplateId,
                signatureRequestTemplate.RecipientName, 
                signatureRequestTemplate.RecipientEmail,
                signatureRequestTemplate.TemplateRole);

            return envelope;
        }

        private static async Task CheckAuthInfo()
        {
            if (_auth == null || string.IsNullOrEmpty(_baseUrl) || string.IsNullOrEmpty(_docuSignCredentials))
            {
                _auth = new AuthenticationClient(Username, Password, IntegratorKey);
                await _auth.LoginInformationAsync();

                _baseUrl = _auth.BaseUrl;
                _docuSignCredentials = _auth.DocuSignCredentials;
            }
        }
    }
}
