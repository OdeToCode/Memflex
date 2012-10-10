using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlexProviders.Roles;

namespace FlexProviders.Tests.Integration.Raven
{
    public class Role : IFlexRole<string>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="Role" /> class.
        /// </summary>
        public Role()
        {
            Users = new Collection<string>();
        }

        #region IFlexRole<string> Members

        /// <summary>
        ///   Gets or sets the users.
        /// </summary>
        /// <value> The users. </value>
        public ICollection<string> Users { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value> The name. </value>
        public string Name { get; set; }

        #endregion
    }
}