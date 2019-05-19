using System;
using Prototype.Models.Content;

namespace Prototype.Models.Navigation
{
	public interface INavigationSettings
	{
		#region Properties

		int? Depth { get; }
		bool ExpandAll { get; }
		bool IncludeRoot { get; }
		Func<IContentNode, string> TextResolver { get; }

		#endregion
	}
}