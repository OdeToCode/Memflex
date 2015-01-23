using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.Membership
{
    public interface IFlexOAuthProvider<TUser> where TUser: IFlexMembershipUser
    {
        /// <summary>
        /// Gets the registered client data.
        /// </summary>
        /// <value>
        /// The registered client data.
        /// </value>
        ICollection<AuthenticationClientData> RegisteredClientData { get; }
        
        /// <summary>
        /// Attempts to perform an OAuth Login.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="providerUserId">The provider user id.</param>
        /// <param name="persistCookie">if set to <c>true</c> [persist cookie].</param>
        /// <returns></returns>
        bool OAuthLogin(string provider, string providerUserId, bool persistCookie);
        
        /// <summary>
        /// Requests the OAuth authentication.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
        void RequestOAuthAuthentication(string provider, string returnUrl);

        /// <summary>
        ///   Verifies the OAuth authentication.
        /// </summary>
        /// <param name="action"> The action. </param>
        /// <returns> </returns>
        AuthenticationResult VerifyOAuthAuthentication(string action);

        /// <summary>
        ///   Creates the OAuth account.
        /// </summary>
        /// <param name="provider"> The provider. </param>
        /// <param name="providerUserId"> The provider user id. </param>
        /// <param name="user"> The user. </param>
        void CreateOAuthAccount(string provider, string providerUserId, TUser user);

        /// <summary>
        ///   Disassociates the OAuth account for a userid.
        /// </summary>
        /// <param name="provider"> The provider. </param>
        /// <param name="providerUserId"> The provider user id. </param>
		/// <param name="group">The group the user belongs to.</param>
        /// <returns> </returns>
        bool DisassociateOAuthAccount(string provider, string providerUserId, string group = null);

        /// <summary>
        ///   Gets the OAuth client data for a provider
        /// </summary>
        /// <param name="provider"> The provider. </param>
        /// <returns> </returns>
        AuthenticationClientData GetOAuthClientData(string provider);

        /// <summary>
        ///   Gets the name of the OAuth accounts for a user.
        /// </summary>
        /// <param name="username"> The username. </param>
		/// <param name="group">The group the user belongs to.</param>
        /// <returns> </returns>
        IEnumerable<OAuthAccount> GetOAuthAccountsFromUserName(string username, string group = null);
    }
}