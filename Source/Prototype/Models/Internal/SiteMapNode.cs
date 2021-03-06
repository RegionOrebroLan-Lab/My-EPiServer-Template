﻿using System.Collections.Generic;
using System.Globalization;

namespace Prototype.Models.Internal
{
	public class SiteMapNode
	{
		#region Properties

		public virtual ICollection<SiteMapNode> Children { get; } = new List<SiteMapNode>();
		public virtual CultureInfo Culture { get; set; }
		public virtual ICollection<CultureInfo> Cultures { get; } = new List<CultureInfo>();
		public virtual string Description { get; set; }
		public virtual string Heading { get; set; }
		public virtual string HtmlContent { get; set; }
		public virtual int Index { get; set; }
		public virtual string Introduction { get; set; }
		public virtual ICollection<string> Keywords { get; } = new List<string>();
		public virtual string Name { get; set; }
		public virtual string Path { get; set; }
		public virtual string Title { get; set; }
		public virtual string UrlSegment { get; set; }

		#endregion
	}
}