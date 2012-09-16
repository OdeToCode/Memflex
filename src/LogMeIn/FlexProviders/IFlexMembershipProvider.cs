namespace FlexProviders
{
    public interface IFlexMembershipProvider
    {        
        bool Login(string username, string password);
        void Logout();
        void CreateAccount(IFlexMembershipUser user);
        bool HasLocalAccount(string username);
        string GetUserName(string provider, string providerUserId);        
        bool ChangePassword(string username, string oldPassword, string newPassword);                
    }
}