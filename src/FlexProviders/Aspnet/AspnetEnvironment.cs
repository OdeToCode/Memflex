using System.Web;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using FlexProviders.Membership;

namespace FlexProviders.Aspnet
{
    public class AspnetEnvironment : IApplicationEnvironment
    {
        #region IApplicationEnvironment Members

        /// <summary>
        ///   Issues the an authorization ticket.
        /// </summary>
        /// <param name="username"> The username. </param>
        /// <param name="persist"> if set to <c>true</c> [persist]. </param>
        public void IssueAuthTicket(string username, bool persist)
        {
            FormsAuthentication.SetAuthCookie(username, persist);
        }

        /// <summary>
        ///   Revokes the authorization ticket.
        /// </summary>
        public void RevokeAuthTicket()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        ///   Acquires the context.
        /// </summary>
        /// <returns> </returns>
        public HttpContextBase AcquireContext()
        {
            return new HttpContextWrapper(HttpContext.Current);
        }

        /// <summary>
        ///   Requests the authentication.
        /// </summary>
        /// <param name="client"> The client. </param>
        /// <param name="provider"> The provider. </param>
        /// <param name="returnUrl"> The return URL. </param>
        public void RequestAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl)
        {
            var securityManager = new OpenAuthSecurityManager(
                new HttpContextWrapper(HttpContext.Current), client, provider);
            securityManager.RequestAuthentication(returnUrl);
        }


        /// <summary>
        /// Gets the name of the OAuth povider.
        /// </summary>
        /// <returns></returns>
        public string GetOAuthPoviderName()
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            return OpenAuthSecurityManager.GetProviderName(context);
        }

        /// <summary>
        ///   Verifies the authentication.
        /// </summary>
        /// <param name="client"> The client. </param>
        /// <param name="provider"> The provider. </param>
        /// <param name="returnUrl"> The return URL. </param>
        /// <returns> </returns>
        public AuthenticationResult VerifyAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider,
                                                         string returnUrl)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            var securityManager = new OpenAuthSecurityManager(context, client, provider);
            return securityManager.VerifyAuthentication(returnUrl);
        }

        #endregion
    }
}