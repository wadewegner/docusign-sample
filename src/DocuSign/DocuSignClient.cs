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

        public DocuSignClient(AuthenticationClient authenticationClient)
            : this (authenticationClient, new HttpClient())
        {
        }

        public DocuSignClient(AuthenticationClient authenticationClient, HttpClient httpClient)
        {
            _baseUrl = authenticationClient.BaseUrl;
            _docuSignCredentials = authenticationClient.DocuSignCredentials;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("X-DocuSign-Authentication", authenticationClient.DocuSignCredentials);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //GetEnvelopeInformationAsync
        public async Task<Envelope> GetEnvelopeInformationAsync(string envelopeId)
        {
            var url = _baseUrl + "/envelopes/" + envelopeId;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
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
                                "<clientUserId>1</clientUserId>" +      // is this always needed? TODO: What do I do with this?
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

        public async Task<RecipientView> RecipientViewAsync(string recipientName, string recipientEmail, string uri)
        {
            var url = _baseUrl + uri + "/views/recipient";

            var requestBody = "<recipientViewRequest xmlns=\"http://www.docusign.com/restapi\">" +
                        "<authenticationMethod>email</authenticationMethod>" +
                        "<email>" + recipientEmail + "</email>" +
                        "<returnUrl>http://www.docusign.com</returnUrl>" +
                        "<clientUserId>1</clientUserId>" + 	    // must match clientUserId set in step 2! TODO: What do I do with this?
                        "<userName>" + recipientName + "</userName>" +
                        "</recipientViewRequest>";

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
                var envelope = JsonConvert.DeserializeObject<RecipientView>(response);
                return envelope;
            }

            // impelmenet exception
            return null;
        }
    }
}
