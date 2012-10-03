using System.Collections.Generic;
using FlexProviders.Mongo;

namespace LogMeIn.Models
{
    public class Role : IFlexRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Users { get; set; }
    }
}