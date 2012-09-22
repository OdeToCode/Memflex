using System.Web;
using DotNetOpenAuth.AspNet;
using FlexProviders;
using FlexProviders.Membership;

namespace LogMeIn.Tests.Integration
{
    public class FakeApplicationEnvironment : IApplicationEnvironment
    {
        public void IssueAuthTicket(string username, bool persist)
        {
            
        }

        public void RevokeAuthTicket()
        {
            
        }

        public string GetOAuthPoviderName()
        {
            return "";
        }

        public void RequestAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl)
        {
            
        }

        public AuthenticationResult VerifyAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl)
        {
            return new AuthenticationResult(true);
        }

        public HttpContextBase AcquireContext()
        {
            return new FakeHttpContext();
        }
        
        public class FakeHttpContext : HttpContextBase
        {

        }
    }    
}