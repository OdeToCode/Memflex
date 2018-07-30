using System.Security.Principal;

namespace FlexProviders.Membership
{
	public interface IFlexPrincipal : IPrincipal
	{
		string License { get; set; }
	}
}
