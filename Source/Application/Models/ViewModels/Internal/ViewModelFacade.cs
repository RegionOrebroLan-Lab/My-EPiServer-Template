using System;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using MyCompany.MyWebApplication.Models.ViewModels.Shared;
using RegionOrebroLan.EPiServer;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	[ServiceConfiguration(typeof(IViewModelFacade), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class ViewModelFacade : IViewModelFacade
	{
		#region Constructors

		public ViewModelFacade(IContentFacade contentFacade, ILayoutFactory layoutFactory, LocalizationService localizationService, ISettings settings)
		{
			this.ContentFacade = contentFacade ?? throw new ArgumentNullException(nameof(contentFacade));
			this.LayoutFactory = layoutFactory ?? throw new ArgumentNullException(nameof(layoutFactory));
			this.LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));

			if(contentFacade.Web?.HttpContext == null)
				throw new ArgumentException("The http-context-property, of the web-facade, can not be null.", nameof(contentFacade));
		}

		#endregion

		#region Properties

		public virtual IContentFacade ContentFacade { get; }
		public virtual ILayoutFactory LayoutFactory { get; }
		public virtual LocalizationService LocalizationService { get; }
		public virtual ISettings Settings { get; }

		#endregion
	}
}