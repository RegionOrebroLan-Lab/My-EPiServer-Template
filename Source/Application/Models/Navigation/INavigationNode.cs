using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public interface INavigationNode : IEnumerable<INavigationNode>
	{
		#region Properties

		/// <summary>
		/// Expanded child-nodes.
		/// </summary>
		IEnumerable<INavigationNode> Children { get; }

		IContent Content { get; }

		/// <summary>
		/// Expanded descendants.
		/// </summary>
		IEnumerable<INavigationNode> Descendants { get; }

		/// <summary>
		/// If the node has a descendant that is selected. This can be true even if the node is a leaf or have no children.
		/// </summary>
		bool DescendantSelected { get; }

		bool Exclude { get; }

		/// <summary>
		/// If the node is a leaf or not. If the node is a leaf it has no children to expand. If the node is not a leaf it has children to expand, but the children property can still be empty if the node is not expanded.
		/// </summary>
		bool Leaf { get; }

		int Level { get; }
		INavigationNode Parent { get; }
		bool Selected { get; }
		string Text { get; }

		#endregion
	}
}