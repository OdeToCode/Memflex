namespace FlexProviders
{
    public interface IFlexOAuthUserRepository
    {
        IFlexOAuthUser GetUserByOAuthProvider(string provider, string providerUserId);
        IFlexOAuthUser DeleteOAuthAccount(string provider, string providerUserId);
        IFlexOAuthUser GetUserByUsername(string ownerAccount);
    }
}