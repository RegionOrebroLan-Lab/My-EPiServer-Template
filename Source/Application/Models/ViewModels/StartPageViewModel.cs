using EPiServer.ServiceLocation;
using MyCompany.MyWebApplication.Models.Pages;
using MyCompany.MyWebApplication.Models.ViewModels.Internal;

namespace MyCompany.MyWebApplication.Models.ViewModels
{
	[ServiceConfiguration(typeof(IStartPageViewModel), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class StartPageViewModel : PageViewModel<StartPage>, IStartPageViewModel
	{
		#region Constructors

		public StartPageViewModel(StartPage content, IViewModelFacade facade) : base(content, facade) { }

		#endregion
	}
}