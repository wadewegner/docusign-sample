using System.Threading.Tasks;
using System.Web.Http;
using DocuSign.Models;
using DocuSign.Web.Utils;

namespace DocuSign.Web.Controllers
{
    public class EnvelopeController : DocuSignController
    {
        // GET api/envelope/<id>
        public async Task<Envelope> Get([FromUri]string id)
        {
            await CheckAuthInfo();
            var client = new DocuSignClient(BaseUrl, DocuSignCredentials);

            var envelopeDetails = await client.GetEnvelopeInformationAsync(id);

            return envelopeDetails;
        }
    }
}
