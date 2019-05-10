using System;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using MyCompany.MyWebApplication.Models.ViewModels.Internal;

namespace MyCompany.MyWebApplication.Controllers.Internal
{
	public abstract class SiteContentController<T> : ContentController<T> where T : IContent
	{
		#region Constructors

		protected SiteContentController(IControllerFacade facade)
		{
			if(facade == null)
				throw new ArgumentNullException(nameof(facade));

			this.ContentRouteHelper = facade.ContentRouteHelper ?? throw new ArgumentException("The content-route-helper-property can not be null.", nameof(facade));
			this.ViewModelFactory = facade.ViewModelFactory ?? throw new ArgumentException("The view-model-factory-property can not be null.", nameof(facade));
		}

		#endregion

		#region Properties

		protected internal virtual IContentRouteHelper ContentRouteHelper { get; }

		/// <summary>
		/// Gets the current routed, typed, content instance from the content-route-helper.
		/// </summary>
		protected internal virtual T RoutedContent => (T) this.ContentRouteHelper.Content;

		protected internal virtual IViewModelFactory ViewModelFactory { get; }

		#endregion
	}
}