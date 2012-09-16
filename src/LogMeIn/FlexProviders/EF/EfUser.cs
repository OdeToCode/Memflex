using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.EF
{
    public class EfUser : IFlexMembershipUser, IFlexOAuthUser
    {
        public string Id { get; set; }
        public string Username { get; set; }        
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocal { get; set; }
        public string Provider { get; set; }
        public string ProviderUsername { get; set; }
        public IEnumerable<OAuthAccount> OAuthAccounts { get; set; }
    }
}