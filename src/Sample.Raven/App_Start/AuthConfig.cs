using System.Collections.Generic;
using DotNetOpenAuth.AspNet.Clients;
using FlexProviders.Membership;
using LogMeIn.Raven.Models;

namespace LogMeIn.Raven
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();

            FlexMembershipProvider<User>.RegisterClient(
                new GoogleOpenIdClient(),
                "Google", new Dictionary<string, object>());
        }
    }
}