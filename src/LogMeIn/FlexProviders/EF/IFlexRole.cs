using System.Collections.Generic;

namespace FlexProviders.EF
{
    public interface IFlexRole<TUser>
    {
        string Name { get; set; }
        ICollection<TUser> Users { get; set; }
    }
}