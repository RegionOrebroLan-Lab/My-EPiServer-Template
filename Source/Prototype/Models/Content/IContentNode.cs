using System;
using System.Collections.Generic;
using System.Globalization;

namespace Prototype.Models.Content
{
	public interface IContentNode
	{
		#region Properties

		IEnumerable<IContentNode> Ancestors { get; }
		IEnumerable<IContentNode> Children { get; }
		CultureInfo Culture { get; }
		IEnumerable<CultureInfo> Cultures { get; }
		IEnumerable<IContentNode> Descendants { get; }
		string Description { get; }
		string Heading { get; }
		string HtmlContent { get; }
		int Index { get; }
		IEnumerable<string> Keywords { get; }
		string Name { get; }
		string Path { get; }
		string Title { get; }
		Uri Url { get; }
		string UrlSegment { get; }

		#endregion
	}
}