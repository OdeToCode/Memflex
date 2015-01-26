using System.Security.Principal;

namespace FlexProviders.Membership
{
	public interface IFlexPrincipal : IPrincipal
	{
		string Group { get; set; }
	}
}
