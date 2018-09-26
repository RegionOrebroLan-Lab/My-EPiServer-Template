using System;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;

namespace MyCompany.MyWebApplication.Controllers
{
	public abstract class SiteContentController<T> : ContentController<T> where T : IContent
	{
		#region Constructors

		protected SiteContentController(IContentRouteHelper contentRouteHelper)
		{
			this.ContentRouteHelper = contentRouteHelper ?? throw new ArgumentNullException(nameof(contentRouteHelper));
		}

		#endregion

		#region Properties

		protected internal virtual IContentRouteHelper ContentRouteHelper { get; }
		protected internal virtual T CurrentContent => (T) this.ContentRouteHelper.Content;

		#endregion
	}
}