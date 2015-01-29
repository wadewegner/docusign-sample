using System.Threading.Tasks;
using DocuSign.Models;
using DocuSign.Web.Utils;

namespace DocuSign.Web.Controllers
{
    public class RecipientController : DocuSignController
    {
        // GET api/envelope/<id>
        public async Task<Recipient> Get(string id)
        {
            await CheckAuthInfo();
            var client = new DocuSignClient(BaseUrl, DocuSignCredentials);

            var recipient = await client.GetEnvelopeRecipientInformationAsync(id);

            return recipient;
        }
    }
}
