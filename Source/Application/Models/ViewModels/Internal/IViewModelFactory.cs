using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public interface IViewModelFactory
	{
		#region Methods

		T Create<T>() where T : IViewModel;
		T Create<T, TContent>(TContent content) where T : IContentViewModel<TContent> where TContent : IContent;

		#endregion
	}
}