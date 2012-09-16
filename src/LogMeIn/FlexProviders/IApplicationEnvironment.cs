using DotNetOpenAuth.AspNet;

namespace FlexProviders
{
    public interface IApplicationEnvironment
    {
        void IssueAuthTicket(string username, bool persist);
        void RevokeAuthTicket();
        string GetOAuthPoviderName();
        void RequestAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl);        
        AuthenticationResult VerifyAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl);        
    }
}
