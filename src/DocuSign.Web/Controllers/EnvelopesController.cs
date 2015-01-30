using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DocuSign.Models;
using DocuSign.Web.Utils;

namespace DocuSign.Web.Controllers
{
    public class EnvelopesController : DocuSignController
    {
        // GET api/envelope/<id>
        public async Task<Envelopes> Get([FromUri]DateTime fromDate)
        {
            await CheckAuthInfo();
            var client = new DocuSignClient(BaseUrl, DocuSignCredentials);

            var envelopes = await client.GetEnvelopesAsync(fromDate);

            return envelopes;
        }
    }
}
