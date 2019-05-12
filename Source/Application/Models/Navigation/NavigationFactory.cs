using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using MyCompany.MyWebApplication.Business.Filters;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	[ServiceConfiguration(typeof(INavigationFactory), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class NavigationFactory : INavigationFactory
	{
		#region Constructors

		public NavigationFactory(IContentLoader contentLoader, IContentRouteHelper contentRouteHelper, ILoggerFactory loggerFactory, IPublishedStateAssessor publishedStateAssessor) : this(new CompositeFilter(new FilterPublished(publishedStateAssessor), new FilterAccess()), contentLoader, contentRouteHelper, loggerFactory) { }

		protected internal NavigationFactory(IContentFilter accessFilter, IContentLoader contentLoader, IContentRouteHelper contentRouteHelper, ILoggerFactory loggerFactory)
		{
			this.AccessFilter = accessFilter ?? throw new ArgumentNullException(nameof(accessFilter));
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.ContentRouteHelper = contentRouteHelper ?? throw new ArgumentNullException(nameof(contentRouteHelper));

			if(loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			this.Logger = loggerFactory.Create(this.GetType().FullName);
		}

		#endregion

		#region Properties

		protected internal virtual IContentFilter AccessFilter { get; }
		protected internal virtual IContentLoader ContentLoader { get; }
		protected internal virtual IContentRouteHelper ContentRouteHelper { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual INavigationRoot Create(ContentReference root, INavigationSettings settings)
		{
			if(settings == null)
				throw new ArgumentNullException(nameof(settings));

			var activeLink = this.ContentRouteHelper.ContentLink;
			var activeLinkAncestors = (ContentReference.IsNullOrEmpty(activeLink) ? Enumerable.Empty<ContentReference>() : this.ContentLoader.GetAncestors(activeLink).Select(ancestor => ancestor.ContentLink)).ToArray();
			IContent content = null;

			if(!ContentReference.IsNullOrEmpty(root))
			{
				try
				{
					content = this.ContentLoader.Get<IContent>(root);

					if(this.AccessFilter.ShouldFilter(content))
					{
						content = null;

						if(this.Logger.IsWarningEnabled())
							this.Logger.Warning("The current user do not have access to root \"{0}\".", root);
					}
				}
				catch(Exception exception)
				{
					if(this.Logger.IsErrorEnabled())
						this.Logger.Error(string.Format(CultureInfo.InvariantCulture, "Could not get content for root \"{0}\".", root), exception);
				}
			}
			else
			{
				if(this.Logger.IsWarningEnabled())
					this.Logger.Warning("The root is {0}.", root == null ? "null" : "empty");
			}

			return new NavigationRoot(activeLink, activeLinkAncestors, this.ContentLoader, content, settings);
		}

		#endregion
	}
}