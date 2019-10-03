using EPiServer.Framework.Localization;
using MyCompany.MyWebApplication.Models.ViewModels.Shared;
using RegionOrebroLan.EPiServer;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public interface IViewModelFacade
	{
		#region Properties

		IContentFacade ContentFacade { get; }
		ILayoutFactory LayoutFactory { get; }
		LocalizationService LocalizationService { get; }
		ISettings Settings { get; }

		#endregion
	}
}