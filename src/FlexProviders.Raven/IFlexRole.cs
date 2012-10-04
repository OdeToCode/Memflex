using System.Collections.Generic;

namespace FlexProviders.Raven
{
    public interface IFlexRole<TUser>
    {
        string Name { get; set; }
        ICollection<string> Users { get; set; }
    }
}