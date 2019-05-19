using System;
using Prototype.Models.Content;

namespace Prototype.Models.Navigation
{
	public class NavigationSettings : INavigationSettings
	{
		#region Properties

		public virtual int? Depth { get; set; }
		public virtual bool ExpandAll { get; set; }
		public virtual bool IncludeRoot { get; set; }
		public virtual Func<IContentNode, string> TextResolver { get; set; } = content => content?.Name;

		#endregion
	}
}