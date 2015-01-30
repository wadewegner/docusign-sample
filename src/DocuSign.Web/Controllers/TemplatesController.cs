using System.Threading.Tasks;
using DocuSign.Models;
using DocuSign.Web.Utils;

namespace DocuSign.Web.Controllers
{
    public class TemplatesController : DocuSignController
    {
        // GET api/templates
        public async Task<Templates> Get()
        {
            await CheckAuthInfo();
            var client = new DocuSignClient(BaseUrl, DocuSignCredentials);

            var templates = await client.GetTemplatesAsync();

            return templates;
        }
    }
}
