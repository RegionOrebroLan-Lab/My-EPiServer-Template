using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Filters;

namespace MyCompany.MyWebApplication.Business.Filters
{
	public abstract class ContentFilter : IContentFilter
	{
		#region Methods

		public virtual void Filter(IList<IContent> contents)
		{
			if(contents == null)
				throw new ArgumentNullException(nameof(contents));

			for(var i = contents.Count - 1; i >= 0; i--)
			{
				if(this.ShouldFilter(contents[i]))
					contents.RemoveAt(i);
			}
		}

		public virtual void Filter(object sender, ContentFilterEventArgs e)
		{
			if(e == null)
				throw new ArgumentNullException(nameof(e));

			this.Filter(e.Contents);
		}

		public abstract bool ShouldFilter(IContent content);

		#endregion
	}
}