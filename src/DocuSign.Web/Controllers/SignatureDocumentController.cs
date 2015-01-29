using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DocuSign.Models;
using DocuSign.Web.Utils;

namespace DocuSign.Web.Controllers
{
    public class SignatureDocumentController : DocuSignController
    {  
        public async Task<Envelope> PostFormData()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            await CheckAuthInfo();

            var client = new DocuSignClient(BaseUrl, DocuSignCredentials);

            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var streamProvider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(streamProvider);

            var documentName = string.Empty;
            var recipientName = string.Empty;
            var recipientEmail = string.Empty;
            var contentType = string.Empty;

            foreach (var key in streamProvider.FormData.AllKeys)
            {
                var strings = streamProvider.FormData.GetValues(key);
                if (strings != null)
                {

                    foreach (var val in strings)
                    {
                        if (key == "DocumentName") documentName = val;
                        if (key == "RecipientName") recipientName = val;
                        if (key == "RecipientEmail") recipientEmail = val;
                        if (key == "ContentType") contentType = val;
                    }
                }
            }

            var fileData = streamProvider.FileData[0];
            var fileInfo = new FileInfo(fileData.LocalFileName);
            var fileStream = fileInfo.OpenRead();

            var envelope = await client.SendDocumentSignatureRequestAsync(documentName, recipientName, recipientEmail, contentType, fileStream);
      
            return envelope;
        }
    }
}
