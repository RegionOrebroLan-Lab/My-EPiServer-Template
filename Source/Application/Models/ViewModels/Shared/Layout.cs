using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using EPiServer.Core;
using MyCompany.MyWebApplication.Models.Navigation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	[SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
	public class Layout : ILayout
	{
		#region Fields

		private bool? _includeRightArea;

		#endregion

		#region Constructors

		public Layout() { }

		public Layout(IContent content)
		{
			this.Content = content ?? throw new ArgumentNullException(nameof(content));
		}

		#endregion

		#region Properties

		public virtual string BodyClass { get; set; }
		public virtual string BodyId { get; set; }
		protected internal virtual IContent Content { get; }
		public virtual CultureInfo Culture { get; set; }
		public virtual IDictionary<CultureInfo, Uri> CultureNavigation { get; } = new Dictionary<CultureInfo, Uri>();
		public virtual string Description { get; set; }

		public virtual bool IncludeRightArea
		{
			get
			{
				if(this._includeRightArea == null)
					this._includeRightArea = this.Content is IRightAreaContent rightAreaContent && rightAreaContent.RightArea != null && rightAreaContent.RightArea.FilteredItems.Any();

				return this._includeRightArea.Value;
			}
		}

		public virtual IList<string> Keywords { get; } = new List<string>();
		public virtual INavigationNode MainNavigation { get; set; }
		public virtual INavigationNode SubNavigation { get; set; }
		public virtual string Title { get; set; }

		#endregion
	}
}