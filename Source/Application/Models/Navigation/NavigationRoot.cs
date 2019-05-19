using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EPiServer;
using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class NavigationRoot : NavigationNode
	{
		#region Constructors

		public NavigationRoot(ContentReference activeLink, IEnumerable<ContentReference> activeLinkAncestors, IContentLoader contentLoader, IContent root, INavigationSettings settings) : base(activeLink, activeLinkAncestors, root, contentLoader, 0, null, settings) { }

		#endregion

		#region Properties

		public override bool Include => this.Content != null && this.Settings.IncludeRoot;

		#endregion
	}
}