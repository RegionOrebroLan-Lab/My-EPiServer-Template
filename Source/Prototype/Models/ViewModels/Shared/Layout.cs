using System.Collections.Generic;
using System.Globalization;
using Prototype.Models.Navigation;

namespace Prototype.Models.ViewModels.Shared
{
	public class Layout : ILayout
	{
		#region Properties

		public virtual CultureInfo Culture { get; set; } = CultureInfo.CurrentUICulture;
		public virtual string Description { get; set; }
		public virtual IList<string> Keywords { get; } = new List<string>();
		public virtual INavigationNode MainNavigation { get; set; }
		public virtual INavigationNode SubNavigation { get; set; }
		public virtual string Title { get; set; }

		#endregion
	}
}