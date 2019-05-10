using MyCompany.MyWebApplication.Controllers.Internal;
using MyCompany.MyWebApplication.Models.Pages;
using MyCompany.MyWebApplication.Models.ViewModels;

namespace MyCompany.MyWebApplication.Controllers
{
	public class StartPageController : SitePageController<StartPage, IStartPageViewModel>
	{
		#region Constructors

		public StartPageController(IControllerFacade facade) : base(facade) { }

		#endregion
	}
}