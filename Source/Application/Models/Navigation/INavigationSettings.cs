using System;
using EPiServer.Core;
using EPiServer.Filters;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	public interface INavigationSettings
	{
		#region Properties

		int? Depth { get; }
		bool ExpandAll { get; }
		IContentFilter Filter { get; }
		bool IncludeRoot { get; }
		Func<IContent, string> TextResolver { get; }

		#endregion
	}
}