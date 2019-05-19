using System;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using MyCompany.MyWebApplication.Models.Navigation;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(typeof(ILayoutFactory), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class LayoutFactory : ILayoutFactory
	{
		#region Fields

		private static INavigationSettings _mainNavigationSettings;
		private static INavigationSettings _subNavigationSettings;

		#endregion

		#region Constructors

		public LayoutFactory(IContentLoader contentLoader, INavigationFactory navigationFactory, ISiteDefinitionResolver siteDefinitionResolver)
		{
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.NavigationFactory = navigationFactory ?? throw new ArgumentNullException(nameof(navigationFactory));
			this.SiteDefinitionResolver = siteDefinitionResolver ?? throw new ArgumentNullException(nameof(siteDefinitionResolver));
		}

		#endregion

		#region Properties

		protected internal virtual IContentLoader ContentLoader { get; }
		protected internal virtual INavigationSettings MainNavigationSettings => _mainNavigationSettings ?? (_mainNavigationSettings = new NavigationSettings {Depth = 1});
		protected internal virtual INavigationFactory NavigationFactory { get; }
		protected internal virtual ISiteDefinitionResolver SiteDefinitionResolver { get; }
		protected internal virtual INavigationSettings SubNavigationSettings => _subNavigationSettings ?? (_subNavigationSettings = new NavigationSettings());

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

			layout.MainNavigation = this.CreateMainNavigation(startPageLink);
			layout.SubNavigation = this.CreateSubNavigation(content, startPageLink);

			return layout;
		}

		protected internal virtual INavigationNode CreateMainNavigation(ContentReference startPageLink)
		{
			return this.NavigationFactory.Create(startPageLink, this.MainNavigationSettings);
		}

		protected internal virtual INavigationNode CreateSubNavigation(IContent content, ContentReference startPageLink)
		{
			ContentReference subNavigationRoot = null;

			// ReSharper disable InvertIf
			if(content != null && !ContentReference.IsNullOrEmpty(startPageLink))
			{
				// We can check some property on the content to get the root-container for the sub-menu.

				var ancestors = this.ContentLoader.GetAncestors(content.ContentLink).ToArray();

				for(var i = 0; i < ancestors.Length; i++)
				{
					var ancestor = ancestors[i];

					if(ancestor.ContentLink.CompareToIgnoreWorkID(startPageLink))
					{
						subNavigationRoot = i == 0 ? content.ContentLink : ancestors[i - 1].ContentLink;
						break;
					}
				}
			}
			// ReSharper restore InvertIf

			return this.NavigationFactory.Create(subNavigationRoot, this.SubNavigationSettings);
		}

		#endregion
	}
}