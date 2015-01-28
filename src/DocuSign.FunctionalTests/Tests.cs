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
            var loginInformation = await Login();
           
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
        public async void SendSignatureRequest()
        {
            var auth = new AuthenticationClient(username, password, integratorKey);
            await auth.LoginInformation();

            var client = new DocuSignClient(auth);
            var envelope = await client.SendSignatureRequestAsync(templateId, recipientName, recipientEmail, templateRole);

            Assert.IsNotNull(envelope);
            Assert.IsNotNull(envelope.envelopeId);
            Assert.IsNotNull(envelope.status);
            Assert.IsNotNull(envelope.statusDateTime);
            Assert.IsNotNull(envelope.uri);
        }

        private async Task<LoginAccount> Login()
        {
            var auth = new AuthenticationClient(username, password, integratorKey);
            var loginInformation = await auth.LoginInformation();
            return loginInformation;
        }

    }
}
