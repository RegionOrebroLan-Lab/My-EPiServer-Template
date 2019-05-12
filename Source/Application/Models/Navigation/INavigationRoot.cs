using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public interface INavigationRoot : INavigationNode, IEnumerable<INavigationNode>
	{
		#region Properties

		bool Include { get; }

		#endregion
	}
}