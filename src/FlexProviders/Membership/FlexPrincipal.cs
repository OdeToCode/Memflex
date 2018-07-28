using System.Security.Principal;

namespace FlexProviders.Membership
{
	public class FlexPrincipal : IFlexPrincipal
	{
		 public IIdentity Identity { get; private set; }
		public bool IsInRole(string role) { return false; }

		public FlexPrincipal(string username)
		{
			this.Identity = new GenericIdentity(username);
		}

		public string License { get; set; }
	}
}
