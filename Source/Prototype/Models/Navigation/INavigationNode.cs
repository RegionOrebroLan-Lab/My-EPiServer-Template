using System;
using System.Collections.Generic;
using Prototype.Models.Content;

namespace Prototype.Models.Navigation
{
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

		IContentNode Content { get; }
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
		Uri Url { get; }

		#endregion
	}
}