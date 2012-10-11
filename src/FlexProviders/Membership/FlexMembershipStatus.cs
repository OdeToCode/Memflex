namespace FlexProviders.Membership
{
    public enum FlexMembershipStatus
    {
        Success,
        InvalidUserName,
        InvalidPassword,
        InvalidQuestion,
        InvalidAnswer,
        InvalidEmail,
        DuplicateUserName,
        DuplicateEmail,
        UserRejected,
        InvalidProviderUserKey,
        DuplicateProviderUserKey,
        ProviderError,
    }
}