using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders
{
    public interface IFlexOAuthUser
    {
        string Username { get; set; }
        string Provider { get; set; }
        string ProviderUsername { get; set; }        
        
        // todo: figure out how to get rid of this one
        bool IsLocal { get; set; }        
    }
}