using System.Web;
using DotNetOpenAuth.AspNet;

namespace FlexProviders.Membership
{
    public interface IApplicationEnvironment
    {
        /// <summary>
        ///   Issues the auth ticket.
        /// </summary>
        /// <param name="username"> The username. </param>
        /// <param name="persist"> if set to <c>true</c> [persist]. </param>
        void IssueAuthTicket(string username, bool persist);

        /// <summary>
        ///   Revokes the auth ticket.
        /// </summary>
        void RevokeAuthTicket();

        /// <summary>
        ///   Gets the name of the OAuth povider.
        /// </summary>
        /// <returns> </returns>
        string GetOAuthPoviderName();

        /// <summary>
        ///   Requests the authentication.
        /// </summary>
        /// <param name="client"> The client. </param>
        /// <param name="provider"> The provider. </param>
        /// <param name="returnUrl"> The return URL. </param>
        void RequestAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl);

        /// <summary>
        ///   Verifies the authentication.
        /// </summary>
        /// <param name="client"> The client. </param>
        /// <param name="provider"> The provider. </param>
        /// <param name="returnUrl"> The return URL. </param>
        /// <returns> </returns>
        AuthenticationResult VerifyAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider,
                                                  string returnUrl);

        /// <summary>
        ///   Acquires the context.
        /// </summary>
        /// <returns> </returns>
        HttpContextBase AcquireContext();
    }
}