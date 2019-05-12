using System;
using EPiServer.Core;
using EPiServer.Filters;
using MyCompany.MyWebApplication.Business.Filters;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	public class NavigationSettings : INavigationSettings
	{
		#region Properties

		public virtual int? Depth { get; set; }
		public virtual bool ExpandAll { get; set; }
		public virtual IContentFilter Filter { get; set; } = new CompositeFilter(new FilterContentForVisitor(), new VisibleInMenuFilter());
		public virtual bool IncludeRoot { get; set; }
		public virtual Func<IContent, string> TextResolver { get; set; } = content => content?.Name;

		#endregion
	}
}