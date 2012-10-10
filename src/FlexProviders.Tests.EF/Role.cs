using System.Collections.Generic;
using FlexProviders.Roles;

namespace FlexProviders.Tests.Integration.EF
{
    public class Role : IFlexRole<User>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}