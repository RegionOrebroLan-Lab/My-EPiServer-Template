using EPiServer.Web.Routing;
using MyCompany.MyWebApplication.Models.ViewModels.Internal;

namespace MyCompany.MyWebApplication.Controllers.Internal
{
	public interface IControllerFacade
	{
		#region Properties

		IContentRouteHelper ContentRouteHelper { get; }
		IViewModelFactory ViewModelFactory { get; }

		#endregion
	}
}