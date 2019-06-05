using System;
using System.Collections.Generic;
using System.Globalization;
using MyCompany.MyWebApplication.Business.Web.Mvc;
using MyCompany.MyWebApplication.Models.Navigation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	public interface ILayout
	{
		#region Properties

		string BodyClass { get; set; }
		string BodyId { get; set; }
		CultureInfo Culture { get; set; }
		IDictionary<CultureInfo, Uri> CultureNavigation { get; }
		string Description { get; set; }
		bool IncludeRightArea { get; }
		IList<string> Keywords { get; }
		INavigationNode MainNavigation { get; set; }
		IModal Modal { get; set; }
		INavigationNode SubNavigation { get; set; }
		string Title { get; set; }

		#endregion
	}
}