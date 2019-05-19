using System.Collections.Generic;
using System.Globalization;
using MyCompany.MyWebApplication.Models.Navigation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	public interface ILayout
	{
		#region Properties

		CultureInfo Culture { get; set; }
		string Description { get; set; }
		IList<string> Keywords { get; }
		INavigationNode MainNavigation { get; set; }
		INavigationNode SubNavigation { get; set; }
		string Title { get; set; }

		#endregion
	}
}