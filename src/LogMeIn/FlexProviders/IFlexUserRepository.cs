namespace FlexProviders
{
    public interface IFlexUserRepository
    {        
        IFlexMembershipUser GetUserByUsername(string username);
        IFlexMembershipUser Add(IFlexMembershipUser user);
        IFlexMembershipUser Save(IFlexMembershipUser user);        
    }    
}