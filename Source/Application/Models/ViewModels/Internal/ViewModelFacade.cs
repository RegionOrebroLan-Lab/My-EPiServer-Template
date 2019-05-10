using System;
using System.Web;
using EPiServer;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using MyCompany.MyWebApplication.Business.Web;
using MyCompany.MyWebApplication.Models.ViewModels.Shared;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	[ServiceConfiguration(typeof(IViewModelFacade), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class ViewModelFacade : IViewModelFacade
	{
		#region Constructors

		public ViewModelFacade(IContentLoader contentLoader, ILayoutFactory layoutFactory, LocalizationService localizationService, ISettings settings, ISiteDefinitionResolver siteDefinitionResolver, IWebFacade webFacade)
		{
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.LayoutFactory = layoutFactory ?? throw new ArgumentNullException(nameof(layoutFactory));
			this.LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
			this.SiteDefinitionResolver = siteDefinitionResolver ?? throw new ArgumentNullException(nameof(siteDefinitionResolver));

			if(webFacade == null)
				throw new ArgumentNullException(nameof(webFacade));

			var httpContext = webFacade.HttpContext;

			if(httpContext == null)
				throw new ArgumentException("The http-context-property can not be null.", nameof(webFacade));

			this.HttpContext = webFacade.HttpContext;
		}

		#endregion

		#region Properties

		public virtual IContentLoader ContentLoader { get; }
		public virtual HttpContextBase HttpContext { get; }
		public virtual ILayoutFactory LayoutFactory { get; }
		public virtual LocalizationService LocalizationService { get; }
		public virtual ISettings Settings { get; }
		public virtual ISiteDefinitionResolver SiteDefinitionResolver { get; }

		#endregion
	}
}