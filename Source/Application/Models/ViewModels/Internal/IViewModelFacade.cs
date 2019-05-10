using System.Web;
using EPiServer;
using EPiServer.Framework.Localization;
using EPiServer.Web;
using MyCompany.MyWebApplication.Models.ViewModels.Shared;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public interface IViewModelFacade
	{
		#region Properties

		IContentLoader ContentLoader { get; }
		HttpContextBase HttpContext { get; }
		ILayoutFactory LayoutFactory { get; }
		LocalizationService LocalizationService { get; }
		ISettings Settings { get; }
		ISiteDefinitionResolver SiteDefinitionResolver { get; }

		#endregion
	}
}