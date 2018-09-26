using System.Web.Mvc;
using EPiServer.Web.Routing;
using MyCompany.MyWebApplication.Models.Pages;
using MyCompany.MyWebApplication.Models.ViewModels;

namespace MyCompany.MyWebApplication.Controllers
{
	public class DefaultPageController : SitePageController<SitePage>
	{
		#region Fields

		private IPageViewModel<SitePage> _model;

		#endregion

		#region Constructors

		public DefaultPageController(IContentRouteHelper contentRouteHelper) : base(contentRouteHelper) { }

		#endregion

		#region Properties

		protected internal virtual IPageViewModel<SitePage> Model => this._model ?? (this._model = new PageViewModel<SitePage>(this.CurrentContent));

		#endregion

		#region Methods

		public virtual ActionResult Index()
		{
			return this.View(this.ViewPath, this.Model);
		}

		#endregion
	}
}