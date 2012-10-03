using System.Collections.Generic;

namespace FlexProviders.Mongo
{
    public interface IFlexRole
    {
        string Name { get; set; }
        ICollection<string> Users { get; set; }
    }
}