using MyCompany.MyWebApplication.Models.Pages;

namespace MyCompany.MyWebApplication.Models.ViewModels
{
	public class StartPageViewModel : PageViewModel<StartPage>
	{
		#region Constructors

		public StartPageViewModel(StartPage currentPage) : base(currentPage) { }

		#endregion
	}
}