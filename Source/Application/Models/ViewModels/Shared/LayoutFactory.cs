using System;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using MyCompany.MyWebApplication.Business.Filters;
using MyCompany.MyWebApplication.Models.Navigation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(typeof(ILayoutFactory), Lifecycle = ServiceInstanceScope.Singleton)]
	public class LayoutFactory : ILayoutFactory
	{
		#region Fields

		private IContentFilter _navigationFilter;

		#endregion

		#region Constructors

		public LayoutFactory(IContentLoader contentLoader, ISiteDefinitionResolver siteDefinitionResolver)
		{
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.SiteDefinitionResolver = siteDefinitionResolver ?? throw new ArgumentNullException(nameof(siteDefinitionResolver));
		}

		#endregion

		#region Properties

		protected internal virtual IContentLoader ContentLoader { get; }
		protected internal virtual IContentFilter NavigationFilter => this._navigationFilter ?? (this._navigationFilter = new CompositeFilter(new FilterContentForVisitor(), new VisibleInMenuFilter()));
		protected internal virtual ISiteDefinitionResolver SiteDefinitionResolver { get; }

		#endregion

		#region Methods

		public virtual ILayout Create()
		{
			return this.CreateInternal(null);
		}

		public virtual ILayout Create(IContent content)
		{
			if(content == null)
				throw new ArgumentNullException(nameof(content));

			return this.CreateInternal(content);
		}

		protected internal virtual ILayout CreateInternal(IContent content)
		{
			var layout = content == null ? new Layout() : new Layout(content);

			var startPageLink = this.SiteDefinitionResolver.GetByContent(content?.ContentLink, true)?.StartPage;

			// ReSharper disable InvertIf
			if(!ContentReference.IsNullOrEmpty(startPageLink))
			{
				var startPage = this.ContentLoader.Get<IContent>(startPageLink);

				layout.MainNavigation = this.CreateMainNavigation(content, startPage);
				layout.SubNavigation = this.CreateSubNavigation(content, startPage);
			}
			// ReSharper restore InvertIf

			return layout;
		}

		protected internal virtual INavigationNode CreateMainNavigation(IContent content, IContent startPage)
		{
			return new NavigationNode(startPage, this.NavigationFilter, this.ContentLoader, null, content?.ContentLink)
			{
				Exclude = true
			};
		}

		protected internal virtual INavigationNode CreateSubNavigation(IContent content, IContent startPage)
		{
			if(startPage == null)
				throw new ArgumentNullException(nameof(startPage));

			// We can check some property to get the root-container for the sub-menu.

			var contentLink = content?.ContentLink;

			if(ContentReference.IsNullOrEmpty(contentLink))
				return null;

			var ancestors = this.ContentLoader.GetAncestors(contentLink).ToArray();

			// ReSharper disable All
			for(var i = 0; i < ancestors.Length; i++)
			{
				var ancestor = ancestors[i];

				if(ancestor.ContentLink.CompareToIgnoreWorkID(startPage.ContentLink))
				{
					var navigationRoot = i == 0 ? content : ancestors[i - 1];

					return new NavigationNode(navigationRoot, this.NavigationFilter, this.ContentLoader, null, contentLink)
					{
						Exclude = true
					};
				}
			}
			// ReSharper restore All

			return null;
		}

		#endregion
	}
}