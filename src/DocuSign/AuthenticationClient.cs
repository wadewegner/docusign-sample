using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DocuSign.Models;
using Newtonsoft.Json;

namespace DocuSign
{
    public class AuthenticationClient
    {
        public string BaseUrl;
        public string DocuSignCredentials;

        private HttpClient _httpClient;
        private const string LoginInformationUrl = "https://demo.docusign.net/restapi/v2/login_information";

        public AuthenticationClient(string username, string password, string integratorKey)
            : this(username, password, integratorKey, new HttpClient())
        {
        }

        public AuthenticationClient(string username, string password, string integratorKey, HttpClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException("httpClient");

            DocuSignCredentials = CreateDocuSignCredentials(username, password, integratorKey);

            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("X-DocuSign-Authentication", DocuSignCredentials);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string CreateDocuSignCredentials(string username, string password, string integratorKey)
        {
            var docuSignCredentials =
                "<DocuSignCredentials>" +
                    "<Username>" + username + "</Username>" +
                    "<Password>" + password + "</Password>" +
                    "<IntegratorKey>" + integratorKey + "</IntegratorKey>" +
                    "</DocuSignCredentials>";

            return docuSignCredentials;
        }

        public async Task<LoginAccount> LoginInformationAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(LoginInformationUrl)
            };

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var loginInformation = JsonConvert.DeserializeObject<LoginAccounts>(response);
                this.BaseUrl = loginInformation.loginAccounts[0].baseUrl;

                return loginInformation.loginAccounts[0];
            }

            // implement error handling
            return null;
        }
    }
}
