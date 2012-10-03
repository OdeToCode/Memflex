using System.Collections.Generic;
using FlexProviders.Membership;

namespace FlexProviders.Tests.Integration.Raven
{
    public class User : IFlexMembershipUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocal { get; set; }
        public ICollection<FlexOAuthAccount> OAuthAccounts { get; set; }
    }
}