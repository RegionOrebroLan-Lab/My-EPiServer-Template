using System;
using EPiServer.Core;
using EPiServer.Filters;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	public interface INavigationSettings
	{
		#region Properties

		int? Depth { get; set; }
		bool ExpandAll { get; set; }
		IContentFilter Filter { get; set; }
		bool IncludeRoot { get; set; }
		Func<IContent, string> TextResolver { get; set; }

		#endregion
	}
}