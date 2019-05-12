using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EPiServer;
using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class NavigationRoot : NavigationNode, INavigationRoot
	{
		#region Constructors

		public NavigationRoot(ContentReference activeLink, IEnumerable<ContentReference> activeLinkAncestors, IContentLoader contentLoader, IContent root, INavigationSettings settings) : base(activeLink, activeLinkAncestors, root, contentLoader, null, settings) { }

		#endregion

		#region Properties

		public virtual bool Include => this.Content != null && this.Settings.IncludeRoot;

		#endregion

		#region Methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public virtual IEnumerator<INavigationNode> GetEnumerator()
		{
			var self = this.Include ? new[] {this} : Enumerable.Empty<INavigationNode>();

			return self.Concat(this.Descendants).GetEnumerator();
		}

		#endregion
	}
}