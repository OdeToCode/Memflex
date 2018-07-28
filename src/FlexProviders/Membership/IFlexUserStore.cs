using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.Membership
{
    public interface IFlexUserStore<TUser> where TUser: IFlexMembershipUser
    {        
        TUser Add(TUser user);        
        TUser Save(TUser user);
        TUser CreateOAuthAccount(string provider, string providerUserId, TUser user);        
        TUser GetUserByUsername(string username, string license = null);
		TUser GetUserBySsoToken(string token);
		IEnumerable<TUser> GetAllUsers(string license = null);
        TUser GetUserByOAuthProvider(string provider, string providerUserId);        
        IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username, string license = null);
        bool DeleteOAuthAccount(string provider, string providerUserId);
        TUser GetUserByPasswordResetToken(string passwordResetToken);
		bool RenameLicense(string oldName, string newName);
    }    
}