using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DocuSign.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace DocuSign.FunctionalTests
{
    [TestFixture]
    public class Tests
    {
        private string username;
        private string password;
        private string integratorKey;
        private string templateId;
        private string templateRole;
        private string recipientName;
        private string recipientEmail;

        private HttpClient _httpClient;

        [TestFixtureSetUp]
        public void Init()
        {
            username = "7c388de0-24b9-4c43-a0a1-2d2614c5e54c";		        // your account email
            password = "iQC}LGq7O^eNXk5U";		                            // your account password
            integratorKey = "NAXX-26a52107-ef86-4a50-9e71-4a0396ba1c49";    // your account Integrator Key (found on Preferences -> API page)
            templateId = "29CB97E5-DCE1-4C14-91A8-A8317BCD29AD";            // valid templateId from a template in your account
            templateRole = "Signing Role";		                            // template role that exists on above template
            recipientName = "Wade Wegner";		                            // recipient (signer) name
            recipientEmail = "wade.wegner@gmail.com";		                // recipient (signer) email
        }

        [Test]
        public async void LoginIsNotNull()
        {
            var auth = new AuthenticationClient(username, password, integratorKey);
            var loginInformation = await auth.LoginInformationAsync();
           
            Assert.IsNotNull(loginInformation);
            Assert.IsNotNull(loginInformation.accountId);
            Assert.IsNotNull(loginInformation.baseUrl);
            Assert.IsNotNull(loginInformation.email);
            Assert.IsNotNull(loginInformation.isDefault);
            Assert.IsNotNull(loginInformation.name);
            Assert.IsNotNull(loginInformation.siteDescription);
            Assert.IsNotNull(loginInformation.userId);
            Assert.IsNotNull(loginInformation.userName);
        }

        [Test]
        public async void GetEnvelopeInformation()
        {
            var auth = new AuthenticationClient(username, password, integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);
            var envelope = await client.SendSignatureRequestAsync(templateId, recipientName, recipientEmail, templateRole);

            var envelopeDetails = await client.GetEnvelopeInformationAsync(envelope.envelopeId);

            Assert.IsNotNull(envelopeDetails);
            Assert.IsNotNull(envelopeDetails.envelopeId);
            Assert.IsNotNull(envelopeDetails.status);
            Assert.IsNotNull(envelopeDetails.documentsUri);
            Assert.IsNotNull(envelopeDetails.recipientsUri);
            Assert.IsNotNull(envelopeDetails.envelopeUri);
            Assert.IsNotNull(envelopeDetails.emailSubject);
            Assert.IsNotNull(envelopeDetails.customFieldsUri);
            Assert.IsNotNull(envelopeDetails.notificationUri);
            Assert.IsNotNull(envelopeDetails.enableWetSign);
            Assert.IsNotNull(envelopeDetails.allowReassign);
            Assert.IsNotNull(envelopeDetails.createdDateTime);
            Assert.IsNotNull(envelopeDetails.lastModifiedDateTime);
            Assert.IsNotNull(envelopeDetails.sentDateTime);
            Assert.IsNotNull(envelopeDetails.statusChangedDateTime);
            Assert.IsNotNull(envelopeDetails.documentsCombinedUri);
            Assert.IsNotNull(envelopeDetails.certificateUri);
            Assert.IsNotNull(envelopeDetails.templatesUri);
            Assert.IsNotNull(envelopeDetails.purgeState);
        }

        [Test]
        public async void GetEnvelopeRecipientInformation()
        {
            var auth = new AuthenticationClient(username, password, integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);

            var envelope = await client.SendSignatureRequestAsync(templateId, recipientName, recipientEmail, templateRole);
            var recipient = await client.GetEnvelopeRecipientInformationAsync(envelope.envelopeId);

            Assert.IsNotNull(recipient);
            Assert.IsNotNull(recipient.signers);
            Assert.IsNotNull(recipient.recipientCount);
            Assert.IsNotNull(recipient.currentRoutingOrder);
            Assert.IsNotNull(recipient.signers[0].name);
            Assert.IsNotNull(recipient.signers[0].email);
            Assert.IsNotNull(recipient.signers[0].recipientId);
            Assert.IsNotNull(recipient.signers[0].recipientIdGuid);
            Assert.IsNotNull(recipient.signers[0].requireIdLookup);
            Assert.IsNotNull(recipient.signers[0].userId);
            Assert.IsNotNull(recipient.signers[0].clientUserId);
            Assert.IsNotNull(recipient.signers[0].routingOrder);
            Assert.IsNotNull(recipient.signers[0].note);
            Assert.IsNotNull(recipient.signers[0].status);
            Assert.IsNotNull(recipient.signers[0].templateLocked);
            Assert.IsNotNull(recipient.signers[0].templateRequired);
        }

        [Test]
        public async void SendSignatureRequestFromDocument()
        {
            var auth = new AuthenticationClient(username, password, integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);
           

        }

        [Test]
        public async void SendSignatureRequestFromTemplate()
        {
            var auth = new AuthenticationClient(username, password, integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);
            var envelope = await client.SendSignatureRequestAsync(templateId, recipientName, recipientEmail, templateRole);

            Assert.IsNotNull(envelope);
            Assert.IsNotNull(envelope.envelopeId);
            Assert.IsNotNull(envelope.status);
            Assert.IsNotNull(envelope.statusDateTime);
            Assert.IsNotNull(envelope.uri);
        }

        [Test]
        public async void GetRecipientViewUrl()
        {
            var auth = new AuthenticationClient(username, password, integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);
            var envelope = await client.SendSignatureRequestAsync(templateId, recipientName, recipientEmail, templateRole);

            var uri = envelope.uri;
            var recipientView = await client.RecipientViewAsync(recipientName, recipientEmail, uri);

            Assert.IsNotNull(recipientView);
            Assert.IsNotNull(recipientView.url);

            Uri uriResult;
            bool isUri = Uri.TryCreate(recipientView.url, UriKind.Absolute, out uriResult);
            Assert.IsTrue(isUri);
        }
    }
}
