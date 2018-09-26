using MyCompany.MyWebApplication.Models.Pages;

namespace MyCompany.MyWebApplication.Models.ViewModels
{
	public interface IPageViewModel<out T> where T : SitePage
	{
		#region Properties

		T CurrentPage { get; }

		#endregion
	}
}