using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.EF
{
    public class EfUser : IFlexMembershipUser, IFlexOAuthUser
    {
        public EfUser()
        {
            // todo: check
            OAuthAccounts = new Collection<EfOAuthAccount>();
        }

        public int Id { get; set; }
        public string Username { get; set; }        
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocal { get; set; }
        public string Provider { get; set; }
        public string ProviderUsername { get; set; }
        public virtual ICollection<EfOAuthAccount> OAuthAccounts { get; set; }        
    }
}