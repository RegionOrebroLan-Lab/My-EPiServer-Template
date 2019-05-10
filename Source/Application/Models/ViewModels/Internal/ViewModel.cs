using System;
using System.Web;
using EPiServer;
using EPiServer.Framework.Localization;
using EPiServer.Web;
using MyCompany.MyWebApplication.Models.ViewModels.Shared;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public abstract class ViewModel : IViewModel
	{
		#region Fields

		private ILayout _layout;

		#endregion

		#region Constructors

		protected ViewModel(IViewModelFacade facade)
		{
			if(facade == null)
				throw new ArgumentNullException(nameof(facade));

			this.ContentLoader = facade.ContentLoader ?? throw new ArgumentException("The content-loader-property can not be null.", nameof(facade));
			this.HttpContext = facade.HttpContext ?? throw new ArgumentException("The http-context-property can not be null.", nameof(facade));
			this.LayoutFactory = facade.LayoutFactory ?? throw new ArgumentException("The layout-factory-property can not be null.", nameof(facade));
			this.LocalizationService = facade.LocalizationService ?? throw new ArgumentException("The localization-service-property can not be null.", nameof(facade));
			this.Settings = facade.Settings ?? throw new ArgumentException("The settings-property can not be null.", nameof(facade));
			this.SiteDefinitionResolver = facade.SiteDefinitionResolver ?? throw new ArgumentException("The site-definition-resolver-property can not be null.", nameof(facade));
		}

		#endregion

		#region Properties

		protected internal virtual IContentLoader ContentLoader { get; }
		protected internal virtual HttpContextBase HttpContext { get; }
		public virtual ILayout Layout => this._layout ?? (this._layout = this.CreateLayout());
		protected internal virtual ILayoutFactory LayoutFactory { get; }
		protected internal virtual LocalizationService LocalizationService { get; }
		public virtual ISettings Settings { get; }
		protected internal virtual ISiteDefinitionResolver SiteDefinitionResolver { get; }

		#endregion

		#region Methods

		protected internal virtual ILayout CreateLayout()
		{
			return this.LayoutFactory.Create();
		}

		public virtual void Initialize() { }

		#endregion
	}
}