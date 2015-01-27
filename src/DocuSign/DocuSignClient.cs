using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DocuSign.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DocuSign
{
    public class DocuSignClient
    {
        private string _baseUrl;
        private string _docuSignCredentials;
        private HttpClient _httpClient;

        public DocuSignClient(string baseUrl, string docuSignCredentials)
            : this (baseUrl, docuSignCredentials, new HttpClient())
        {
        }

        public DocuSignClient(string baseUrl, string docuSignCredentials, HttpClient httpClient)
        {
            _baseUrl = baseUrl;
            _docuSignCredentials = docuSignCredentials;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("X-DocuSign-Authentication", docuSignCredentials);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Envelope> SendSignatureRequestAsync(string templateId, string recipientName, string recipientEmail, string templateRole)
        {
            var url = _baseUrl + "/envelopes";

            var requestBody =
                    "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                        "<status>sent</status>" +
                        "<emailSubject>DocuSign API - Signature Request from Template</emailSubject>" +
                        "<templateId>" + templateId + "</templateId>" +
                        "<templateRoles>" +
                            "<templateRole>" +
                                "<name>" + recipientName + "</name>" +
                                "<email>" + recipientEmail + "</email>" +
                                "<roleName>" + templateRole + "</roleName>" +
                            "</templateRole>" +
                        "</templateRoles>" +
                    "</envelopeDefinition>";

            var content = new StringContent(requestBody, Encoding.UTF8, "application/xml");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = content
            };

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var envelope = JsonConvert.DeserializeObject<Envelope>(response);
                return envelope;
            }

            // impelmenet exception
            return null;
        }
    }
}
