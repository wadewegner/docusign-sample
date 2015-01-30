using System;
using System.Collections.Generic;
using System.IO;
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
        private HttpClient _httpClient;

        public DocuSignClient(AuthenticationClient authenticationClient)
            : this (authenticationClient, new HttpClient())
        {
        }

        public DocuSignClient(AuthenticationClient authenticationClient, HttpClient httpClient)
            : this (authenticationClient.BaseUrl, authenticationClient.DocuSignCredentials, httpClient)
        {
        }

        public DocuSignClient(string baseUrl, string docuSignCredentials)
            : this (baseUrl, docuSignCredentials, new HttpClient())
        {
        }

        public DocuSignClient(string baseUrl, string docuSignCredentials, HttpClient httpClient)
        {
            _baseUrl = baseUrl;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("X-DocuSign-Authentication", docuSignCredentials);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Envelopes> GetEnvelopesAsync(DateTime fromDate)
        {
            var currMonth = fromDate.Month;
            var currDay = fromDate.Day;
            var currYear = fromDate.Year;

            if (currMonth != 1)
            {
                currMonth -= 1;
            }
            else
            {   
                // special case for january
                currMonth = 12;
                currYear -= 1;
            }

            var url = _baseUrl + "/envelopes?from_date=" + currMonth + "%2F" + currDay + "%2F" + currYear;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var envelopes = JsonConvert.DeserializeObject<Envelopes>(response);
                return envelopes;
            }

            // implement exception
            return null;
        }

        public async Task<Templates> GetTemplatesAsync()
        {
            var url = _baseUrl + "/templates";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var envelope = JsonConvert.DeserializeObject<Templates>(response);
                return envelope;
            }

            // implement exception
            return null;
        }

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

            // implement exception
            return null;
        }

        public async Task<Recipient> GetEnvelopeRecipientInformationAsync(string envelopeId)
        {
            var url = _baseUrl + "/envelopes/" + envelopeId + "/recipients";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var envelope = JsonConvert.DeserializeObject<Recipient>(response);
                return envelope;
            }

            // implement exception
            return null;
        }

        public async Task<Envelope> SendDocumentSignatureRequestAsync(string documentName, string recipientName, string recipientEmail, string contentType, FileStream fileStream)
        {
            var url = _baseUrl + "/envelopes";

            var xmlBody =
                "<envelopeDefinition xmlns=\"http://www.docusign.com/restapi\">" +
                "<emailSubject>DocuSign API - Signature Request on Document</emailSubject>" +
                "<status>sent</status>" + 	// "sent" to send immediately, "created" to save as draft in your account
                // add document(s)
                "<documents>" +
                    "<document>" +
                        "<documentId>1</documentId>" +
                        "<name>" + documentName + "</name>" +
                    "</document>" +
                "</documents>" +
                // add recipient(s)
                "<recipients>" +
                    "<signers>" +
                        "<signer>" +
                            "<recipientId>1</recipientId>" +
                            "<email>" + recipientEmail + "</email>" +
                            "<name>" + recipientName + "</name>" +
                            "<tabs>" +
                                "<signHereTabs>" +
                                    "<signHere>" +
                                        "<xPosition>100</xPosition>" + // default unit is pixels
                                        "<yPosition>100</yPosition>" + // default unit is pixels
                                        "<documentId>1</documentId>" +
                                        "<pageNumber>1</pageNumber>" +
                                    "</signHere>" +
                                "</signHereTabs>" +
                            "</tabs>" +
                        "</signer>" +
                    "</signers>" +
                "</recipients>" +
                "</envelopeDefinition>";

            using (var content = new MultipartFormDataContent("BOUNDARY"))
            {
                content.Add(new StringContent(xmlBody, Encoding.UTF8, "application/xml"));
                content.Add(new StreamContent(fileStream), "test", documentName);

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
            }

            // implement exception
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

            // implement exception
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

            // implement exception
            return null;
        }
    }
}
