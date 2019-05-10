using System;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using MyCompany.MyWebApplication.Models.ViewModels.Internal;

namespace MyCompany.MyWebApplication.Controllers.Internal
{
	[ServiceConfiguration(typeof(IControllerFacade), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class ControllerFacade : IControllerFacade
	{
		#region Constructors

		public ControllerFacade(IContentRouteHelper contentRouteHelper, IViewModelFactory viewModelFactory)
		{
			this.ContentRouteHelper = contentRouteHelper ?? throw new ArgumentNullException(nameof(contentRouteHelper));
			this.ViewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
		}

		#endregion

		#region Properties

		public virtual IContentRouteHelper ContentRouteHelper { get; }
		public virtual IViewModelFactory ViewModelFactory { get; }

		#endregion
	}
}