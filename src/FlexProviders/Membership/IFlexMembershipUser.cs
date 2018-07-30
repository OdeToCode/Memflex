using System;
using System.Collections.Generic;

namespace FlexProviders.Membership
{
    public interface IFlexMembershipUser
    {
		//The license the user is part of, null is valid.
		//Used if user "sallen" should exists with two customers of you system, it allows the same user with a license diffrentiator.
		string License { get; set; }

		//Single sign on token
		string SsoAccessToken { get; set; }
		DateTime? SsoTokenExpiration { get; set; }

		string Username { get; set; }
        string Password { get; set; }
        string Salt { get; set; }
        string PasswordResetToken { get; set; }
        DateTime PasswordResetTokenExpiration { get; set; }
        ICollection<FlexOAuthAccount> OAuthAccounts { get; set; }       
    }
}