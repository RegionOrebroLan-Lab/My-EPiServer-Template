using MyCompany.MyWebApplication.Controllers.Internal;
using MyCompany.MyWebApplication.Models.Pages;
using MyCompany.MyWebApplication.Models.ViewModels.Internal;

namespace MyCompany.MyWebApplication.Controllers
{
	public class DefaultPageController : SitePageController<SitePage, IPageViewModel<SitePage>>
	{
		#region Constructors

		public DefaultPageController(IControllerFacade facade) : base(facade) { }

		#endregion
	}
}