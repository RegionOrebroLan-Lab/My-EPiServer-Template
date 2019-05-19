using System.Collections.Generic;
using Prototype.Models.Content;

namespace Prototype.Models.Navigation
{
	public class NavigationRoot : NavigationNode
	{
		#region Constructors

		public NavigationRoot(IContentNode activeContent, IEnumerable<IContentNode> activeContentAncestors, IContentNode content, INavigationSettings settings) : base(activeContent, activeContentAncestors, content, 0, null, settings) { }

		#endregion

		#region Properties

		public override bool Include => this.Content != null && this.Settings.IncludeRoot;

		#endregion
	}
}