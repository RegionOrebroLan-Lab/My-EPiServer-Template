using System;
using System.Globalization;
using System.Web.Mvc;
using EPiServer;
using MyCompany.MyWebApplication.Models.Pages;
using MyCompany.MyWebApplication.Models.ViewModels.Internal;

namespace MyCompany.MyWebApplication.Controllers.Internal
{
	public abstract class SitePageController<T> : SiteContentController<T> where T : SitePage
	{
		#region Fields

		private const string _defaultViewName = "Index";
		private Lazy<string> _defaultViewPath;
		private const string _viewPathFormat = "~/Views/{0}/{1}.cshtml";

		#endregion

		#region Constructors

		protected SitePageController(IControllerFacade facade) : base(facade) { }

		#endregion

		#region Properties

		protected internal virtual string DefaultViewName => _defaultViewName;

		protected internal virtual string DefaultViewPath
		{
			get
			{
				if(this._defaultViewPath == null)
					this._defaultViewPath = new Lazy<string>(() => this.GetViewPath(this.DefaultViewName));

				return this._defaultViewPath.Value;
			}
		}

		protected internal virtual string ViewPathFormat => _viewPathFormat;

		#endregion

		#region Methods

		/// <summary>
		/// Gets the view-path for the view-name.
		/// </summary>
		/// <param name="viewName">The view-name without the file-extension, eg "Index".</param>
		protected internal virtual string GetViewPath(string viewName)
		{
			return string.Format(CultureInfo.InvariantCulture, this.ViewPathFormat, this.RoutedContent.GetOriginalType().Name, viewName);
		}

		#endregion
	}

	public abstract class SitePageController<T, TViewModel> : SitePageController<T> where T : SitePage where TViewModel : IContentViewModel<T>
	{
		#region Fields

		private Lazy<TViewModel> _viewModel;

		#endregion

		#region Constructors

		protected SitePageController(IControllerFacade facade) : base(facade) { }

		#endregion

		#region Properties

		protected internal virtual TViewModel ViewModel
		{
			get
			{
				if(this._viewModel == null)
					this._viewModel = new Lazy<TViewModel>(() => this.ViewModelFactory.Create<TViewModel>());

				return this._viewModel.Value;
			}
		}

		#endregion

		#region Methods

		public virtual ActionResult Index()
		{
			return this.View(this.DefaultViewPath, this.ViewModel);
		}

		#endregion
	}
}