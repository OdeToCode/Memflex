namespace LogMeIn
{
    // TODO: what parameters are needed?
    public interface IUserProfileManager
    {
        bool Exists(string userName);
        void Add(string userName);
    }
}