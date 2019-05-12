using System.Collections.Generic;
using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	public interface INavigationNode
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

		/// <summary>
		/// If the node is a leaf or not. If the node is a leaf it has no children to expand. If the node is not a leaf it has children to expand, but the children property can still be empty if the node is not expanded.
		/// </summary>
		bool Leaf { get; }

		int Level { get; }
		string Text { get; }

		#endregion
	}
}