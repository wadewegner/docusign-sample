using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
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
        private string _username;
        private string _password;
        private string _integratorKey;
        private string _templateId;
        private string _templateRole;
        private string _recipientName;
        private string _recipientEmail;

        [TestFixtureSetUp]
        public void Init()
        {
            _username = "7c388de0-24b9-4c43-a0a1-2d2614c5e54c";             // your account email
            _integratorKey = "NAXX-26a52107-ef86-4a50-9e71-4a0396ba1c49";   // your account Integrator Key (found on Preferences -> API page)
            _templateId = "29CB97E5-DCE1-4C14-91A8-A8317BCD29AD";           // valid templateId from a template in your account
            _templateRole = "Signing Role";		                            // template role that exists on above template
            _recipientName = "Wade Wegner";		                            // recipient (signer) name
            _recipientEmail = "wade.wegner@gmail.com";		                // recipient (signer) email
        }

        [Test]
        public async void LoginIsNotNull()
        {
            var auth = new AuthenticationClient(_username, _password, _integratorKey);
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
            var auth = new AuthenticationClient(_username, _password, _integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);
            var envelope = await client.SendSignatureRequestAsync(_templateId, _recipientName, _recipientEmail, _templateRole);

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
            var auth = new AuthenticationClient(_username, _password, _integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);

            var envelope = await client.SendSignatureRequestAsync(_templateId, _recipientName, _recipientEmail, _templateRole);
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
            var auth = new AuthenticationClient(_username, _password, _integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);

            var documentName = "test.pdf";
            var fileStream = File.OpenRead(documentName);
            var envelope = await client.SendDocumentSignatureRequestAsync(documentName, _recipientName, _recipientEmail, "application/pdf", fileStream);

            Assert.IsNotNull(envelope);
            Assert.IsNotNull(envelope.envelopeId);
            Assert.IsNotNull(envelope.status);
            Assert.IsNotNull(envelope.statusDateTime);
            Assert.IsNotNull(envelope.uri);
        }

        [Test]
        public async void SendSignatureRequestFromTemplate()
        {
            var auth = new AuthenticationClient(_username, _password, _integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);
            var envelope = await client.SendSignatureRequestAsync(_templateId, _recipientName, _recipientEmail, _templateRole);

            Assert.IsNotNull(envelope);
            Assert.IsNotNull(envelope.envelopeId);
            Assert.IsNotNull(envelope.status);
            Assert.IsNotNull(envelope.statusDateTime);
            Assert.IsNotNull(envelope.uri);
        }

        [Test]
        public async void GetRecipientViewUrl()
        {
            var auth = new AuthenticationClient(_username, _password, _integratorKey);
            await auth.LoginInformationAsync();

            var client = new DocuSignClient(auth);
            var envelope = await client.SendSignatureRequestAsync(_templateId, _recipientName, _recipientEmail, _templateRole);

            var uri = envelope.uri;
            var recipientView = await client.RecipientViewAsync(_recipientName, _recipientEmail, uri);

            Assert.IsNotNull(recipientView);
            Assert.IsNotNull(recipientView.url);

            Uri uriResult;
            bool isUri = Uri.TryCreate(recipientView.url, UriKind.Absolute, out uriResult);
            Assert.IsTrue(isUri);
        }
    }
}
