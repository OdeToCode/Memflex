namespace FlexProviders.Membership
{
    public interface IFlexMembershipProvider
    {        
        bool Login(string username, string password);
        void Logout();
        void CreateAccount(IFlexMembershipUser user);
        void UpdateAccount(IFlexMembershipUser user);
        bool HasLocalAccount(string username);                
        bool ChangePassword(string username, string oldPassword, string newPassword);
        void SetLocalPassword(string username, string newPassword);
        string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow = 1440);
        bool ResetPassword(string passwordResetToken, string newPassword);
    }
}