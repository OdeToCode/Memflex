namespace FlexProviders.EF
{
    public class EfUser : IFlexMembershipUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocal { get; set; }
    }
}