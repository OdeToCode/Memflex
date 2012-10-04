using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetOpenAuth.AspNet.Clients;
using FlexProviders.Membership;
using Microsoft.Web.WebPages.OAuth;
using LogMeIn.Models;

namespace LogMeIn
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //FlexMembershipProvider.RegisterClient(
            //              new MicrosoftClient("", ""), 
            //              "Microsoft", new Dictionary<string, object>());

            //FlexMembershipProvider.RegisterClient(
            //              new TwitterClient("", ""),
            //              "Twitter", new Dictionary<string, object>());

            //FlexMembershipProvider.RegisterClient(
            //    new FacebookClient("", ""),
            //    "Facebook", new Dictionary<string, object>());

            FlexMembershipProvider.RegisterClient(
               new GoogleOpenIdClient(),
               "Google", new Dictionary<string, object>());
        }
    }
}
