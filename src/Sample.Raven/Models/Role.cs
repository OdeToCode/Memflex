using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlexProviders.Roles;

namespace LogMeIn.Raven.Models
{
    public class Role : IFlexRole<string>
    {
        public Role()
        {
            Users = new Collection<string>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public ICollection<string> Users { get; set; }
    }
}