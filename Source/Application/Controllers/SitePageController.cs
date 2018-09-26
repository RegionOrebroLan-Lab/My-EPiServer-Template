using System;
using System.Globalization;
using EPiServer;
using EPiServer.Web.Routing;
using MyCompany.MyWebApplication.Models.Pages;

namespace MyCompany.MyWebApplication.Controllers
{
	public abstract class SitePageController<T> : SiteContentController<T> where T : SitePage
	{
		#region Fields

		private Lazy<string> _viewPath;
		private const string _viewPathFormat = "~/Views/{0}/Index.cshtml";

		#endregion

		#region Constructors

		protected SitePageController(IContentRouteHelper contentRouteHelper) : base(contentRouteHelper) { }

		#endregion

		#region Properties

		protected internal virtual string ViewPath
		{
			get
			{
				if(this._viewPath == null)
					this._viewPath = new Lazy<string>(() => string.Format(CultureInfo.InvariantCulture, this.ViewPathFormat, this.CurrentContent.GetOriginalType().Name));

				return this._viewPath.Value;
			}
		}

		protected internal virtual string ViewPathFormat => _viewPathFormat;

		#endregion
	}
}