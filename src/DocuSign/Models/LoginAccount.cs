using System.Collections.Generic;

namespace DocuSign.Models
{
    public class LoginAccount
    {
        public string name { get; set; }
        public string accountId { get; set; }
        public string baseUrl { get; set; }
        public string isDefault { get; set; }
        public string userName { get; set; }
        public string userId { get; set; }
        public string email { get; set; }
        public string siteDescription { get; set; }
    }

    public class LoginAccounts
    {
        public List<LoginAccount> loginAccounts { get; set; }
    }
}
