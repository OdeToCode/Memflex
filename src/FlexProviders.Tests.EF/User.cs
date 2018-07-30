﻿using System;
using System.Collections.Generic;
using FlexProviders.Membership;

namespace FlexProviders.Tests.Integration.EF
{
    public class User : IFlexMembershipUser
    {
        public User()
        {
            PasswordResetTokenExpiration = DateTime.Now;
        }

        public int Id { get; set; }
		public string Group { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
		public string Email { get; set; }
        public string Salt { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime PasswordResetTokenExpiration { get; set; }
        public bool IsLocal { get; set; }
        public int FavoriteNumber { get; set; }
        public virtual ICollection<FlexOAuthAccount> OAuthAccounts { get; set; }
        public ICollection<Role> Roles { get; set; }
        public string License { get; set; }
        public string SsoToken { get; set; }
        public DateTime? SsoTokenExpiration { get; set; }
    }
}