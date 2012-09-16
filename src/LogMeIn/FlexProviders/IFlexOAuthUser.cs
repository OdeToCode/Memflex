using System.Collections.Generic;

namespace FlexProviders
{
    public interface IFlexOAuthUser
    {
        string Username { get; set; }
        bool IsLocal { get; set; }
        IEnumerable<object> OAuthAccounts { get; set; } 
    }
}