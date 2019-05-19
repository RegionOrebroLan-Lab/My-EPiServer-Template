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
		/// If the node is the active/current content.
		/// </summary>
		bool Active { get; }

		/// <summary>
		/// If the node is an ancestor to the active/current content.
		/// </summary>
		bool ActiveAncestor { get; }

		/// <summary>
		/// Expanded child-nodes.
		/// </summary>
		IEnumerable<INavigationNode> Children { get; }

		IContent Content { get; }
		bool Include { get; }
		int Index { get; }

		/// <summary>
		/// If the node is a leaf or not. If the node is a leaf it has no children to expand. If the node is not a leaf it has children to expand, but the children property can still be empty if the node is not expanded.
		/// </summary>
		bool Leaf { get; }

		int Level { get; }

		/// <summary>
		/// If the parent is null it is the root node.
		/// </summary>
		INavigationNode Parent { get; }

		string Text { get; }

		#endregion
	}
}