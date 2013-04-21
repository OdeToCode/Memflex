namespace FlexProviders.Membership
{
    public interface IFlexMembershipProvider
    {
        /// <summary>
        /// Determines whether the provided <paramref name="username"/> and
        /// <paramref name="password"/> combination is valid
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="rememberMe">
        /// if set to <c>true</c> [remember me].
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        bool Login(string username, string password, bool rememberMe = false);

        /// <summary>
        ///   Logout the current user
        /// </summary>
        void Logout();

        /// <summary>
        ///   Creates an account.
        /// </summary>
        /// <param name="user"> The user. </param>
        void CreateAccount(IFlexMembershipUser user);

        /// <summary>
        ///   Updates the account.
        /// </summary>
        /// <param name="user"> The user. </param>
        void UpdateAccount(IFlexMembershipUser user);

        /// <summary>
        ///   Determines whether the specific <paramref name="username" /> has a
        ///   local account
        /// </summary>
        /// <param name="username"> The username. </param>
        /// <returns> <c>true</c> if the specified username has a local account; otherwise, <c>false</c> . </returns>
        bool HasLocalAccount(string username);

        /// <summary>
        ///   Determines if a given username already exists
        /// </summary>
        /// <param name="username"> The username. </param>
        /// <returns> <c>true</c> if the specified username exists; otherwise, <c>false</c> . </returns>
        bool Exists(string username);

        /// <summary>
        ///   Changes the password for a user
        /// </summary>
        /// <param name="username"> The username. </param>
        /// <param name="oldPassword"> The old password. </param>
        /// <param name="newPassword"> The new password. </param>
        /// <returns> </returns>
        bool ChangePassword(string username, string oldPassword, string newPassword);

        /// <summary>
        ///   Sets the local password for a user
        /// </summary>
        /// <param name="username"> The username. </param>
        /// <param name="newPassword"> The new password. </param>
        void SetLocalPassword(string username, string newPassword);

        /// <summary>
        ///   Generates the password reset token for a user
        /// </summary>
        /// <param name="username"> The username. </param>
        /// <param name="tokenExpirationInMinutesFromNow"> The token expiration in minutes from now. </param>
        /// <returns> </returns>
        string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow = 1440);

        /// <summary>
        ///   Resets the password for the supplied
        ///   <paramref name="passwordResetToken" />
        /// </summary>
        /// <param name="passwordResetToken"> The password reset token to perform the lookup on. </param>
        /// <param name="newPassword"> The new password for the user. </param>
        /// <returns> </returns>
        bool ResetPassword(string passwordResetToken, string newPassword);
    }
}