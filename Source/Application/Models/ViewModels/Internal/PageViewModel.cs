using EPiServer.ServiceLocation;
using MyCompany.MyWebApplication.Models.Pages;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	[ServiceConfiguration(typeof(IPageViewModel<>), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class PageViewModel<T> : ContentViewModel<T>, IPageViewModel<T> where T : SitePage
	{
		#region Constructors

		public PageViewModel(T content, IViewModelFacade facade) : base(content, facade) { }

		#endregion

		#region Methods

		public override void Initialize()
		{
			base.Initialize();

			this.Layout.Culture = this.Content.Language;
			//this.Layout.Description = this.Content.MetaDescription;
			//this.Layout.Keywords = this.Content.MetaKeywords;
			this.Layout.Title = this.Content.Name;
		}

		#endregion
	}
}