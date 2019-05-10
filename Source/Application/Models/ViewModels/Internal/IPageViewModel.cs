using MyCompany.MyWebApplication.Models.Pages;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public interface IPageViewModel<out T> : IContentViewModel<T> where T : SitePage { }
}