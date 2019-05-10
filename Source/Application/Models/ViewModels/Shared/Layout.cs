using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using EPiServer.Core;
using MyCompany.MyWebApplication.Models.Navigation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	[SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
	public class Layout : ILayout
	{
		#region Constructors

		public Layout() { }

		public Layout(IContent content)
		{
			this.Content = content ?? throw new ArgumentNullException(nameof(content));
		}

		#endregion

		#region Properties

		protected internal virtual IContent Content { get; }
		public virtual CultureInfo Culture { get; set; }
		public virtual string Description { get; set; }
		public virtual IList<string> Keywords { get; } = new List<string>();
		public virtual INavigationNode MainNavigation { get; set; }
		public virtual INavigationNode SubNavigation { get; set; }
		public virtual string Title { get; set; }

		#endregion
	}
}