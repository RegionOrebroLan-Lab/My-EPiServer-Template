using System;
using Prototype.Models.Content;

namespace Prototype.Models.Navigation
{
	public interface INavigationSettings
	{
		#region Properties

		int? Depth { get; set; }
		bool ExpandAll { get; set; }
		bool IncludeRoot { get; set; }
		Func<IContentNode, string> TextResolver { get; set; }

		#endregion
	}
}