using System.Collections.Generic;
using FlexProviders.EF;

namespace LogMeIn.Models
{
    public class Role : IFlexRole<User>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}