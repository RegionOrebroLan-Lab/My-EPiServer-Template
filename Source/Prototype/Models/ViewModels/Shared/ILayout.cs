using System;
using System.Collections.Generic;
using System.Globalization;
using Prototype.Models.Navigation;

namespace Prototype.Models.ViewModels.Shared
{
	public interface ILayout
	{
		#region Properties

		CultureInfo Culture { get; set; }
		IDictionary<CultureInfo, Uri> CultureNavigation { get; }

		/// <summary>
		/// Meta-description
		/// </summary>
		string Description { get; set; }

		/// <summary>
		/// Meta-keywords
		/// </summary>
		IList<string> Keywords { get; }

		INavigationNode MainNavigation { get; set; }
		INavigationNode SubNavigation { get; set; }

		/// <summary>
		/// Meta-title
		/// </summary>
		string Title { get; set; }

		#endregion
	}
}