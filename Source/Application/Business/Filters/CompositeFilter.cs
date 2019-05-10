using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.Filters;

namespace MyCompany.MyWebApplication.Business.Filters
{
	public class CompositeFilter : ContentFilter
	{
		#region Constructors

		public CompositeFilter(IEnumerable<IContentFilter> filters)
		{
			this.Filters = filters ?? Enumerable.Empty<IContentFilter>();
		}

		public CompositeFilter(params IContentFilter[] filters) : this((IEnumerable<IContentFilter>) filters) { }

		#endregion

		#region Properties

		protected internal virtual IEnumerable<IContentFilter> Filters { get; }

		#endregion

		#region Methods

		public override bool ShouldFilter(IContent content)
		{
			return this.Filters.Any(filter => filter.ShouldFilter(content));
		}

		#endregion
	}
}