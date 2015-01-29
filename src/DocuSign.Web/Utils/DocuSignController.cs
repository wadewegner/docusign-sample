using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DocuSign.Web.Utils
{
    public class DocuSignController : ApiController
    {
        private static readonly string Username = ConfigurationManager.AppSettings["Username"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];
        private static readonly string IntegratorKey = ConfigurationManager.AppSettings["IntegratorKey"];

        protected static string BaseUrl;
        protected static string DocuSignCredentials;

        private static AuthenticationClient _auth;

        protected static async Task CheckAuthInfo()
        {
            if (_auth == null || string.IsNullOrEmpty(BaseUrl) || string.IsNullOrEmpty(DocuSignCredentials))
            {
                _auth = new AuthenticationClient(Username, Password, IntegratorKey);
                await _auth.LoginInformationAsync();

                BaseUrl = _auth.BaseUrl;
                DocuSignCredentials = _auth.DocuSignCredentials;
            }
        }
    }
}
