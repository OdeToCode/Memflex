namespace FlexProviders.Membership
{
    public interface IFlexMembershipProvider
    {        
        bool Login(string username, string password);
        void Logout();
        void CreateAccount(IFlexMembershipUser user);
        bool HasLocalAccount(string username);                
        bool ChangePassword(string username, string oldPassword, string newPassword);                
    }
}