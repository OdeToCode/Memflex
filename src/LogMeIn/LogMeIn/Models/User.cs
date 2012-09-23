using System.Collections.Generic;
using FlexProviders.Membership;

namespace LogMeIn.Models
{
    public class User : IFlexMembershipUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocal { get; set; }
        public int FavoriteNumber { get; set; }
        public virtual ICollection<FlexOAuthAccount> OAuthAccounts { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}