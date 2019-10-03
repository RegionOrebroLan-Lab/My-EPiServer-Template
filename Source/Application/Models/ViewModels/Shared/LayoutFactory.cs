using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using MyCompany.MyWebApplication.Models.Pages;
using RegionOrebroLan.EPiServer;
using RegionOrebroLan.EPiServer.Collections;
using EPiServerSettings = EPiServer.Configuration.Settings;

namespace MyCompany.MyWebApplication.Models.ViewModels.Shared
{
	[ServiceConfiguration(typeof(ILayoutFactory), Lifecycle = ServiceInstanceScope.Hybrid)]
	public class LayoutFactory : ILayoutFactory
	{
		#region Fields

		private static ITreeSettings _mainNavigationSettings;
		private static ITreeSettings _subNavigationSettings;

		#endregion

		#region Constructors

		public LayoutFactory(IContentFacade contentFacade, EPiServerSettings settings)
		{
			this.ContentFacade = contentFacade ?? throw new ArgumentNullException(nameof(contentFacade));
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		#endregion

		#region Properties

		protected internal virtual IContentFacade ContentFacade { get; }
		public virtual ITreeSettings MainNavigationSettings => _mainNavigationSettings ?? (_mainNavigationSettings = new TreeSettings {Depth = 1, IncludeRoot = true, IndicateActiveContent = true});
		protected internal virtual EPiServerSettings Settings { get; }
		public virtual ITreeSettings SubNavigationSettings => _subNavigationSettings ?? (_subNavigationSettings = new TreeSettings {IndicateActiveContent = true});

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

		[SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
		protected internal virtual string CreateBodyId(IContent content)
		{
			if(content == null)
				return "page";

			var typeName = content.GetOriginalType().Name;
			var parts = Regex.Split(typeName, @"(?<!^)(?=[A-Z])"); // Split on uppercase.

			return string.Join("-", parts.Select(part => part.ToLowerInvariant()));
		}

		protected internal virtual ILayout CreateInternal(IContent content)
		{
			var layout = content == null ? new Layout() : new Layout(content);

			layout.BodyClass = layout.BodyId = this.CreateBodyId(content);

			this.PopulateCultureNavigation(content, layout.CultureNavigation);

			var startPageLink = this.ContentFacade.SiteDefinitionResolver.GetByContent(content?.ContentLink, true)?.StartPage;

			layout.MainNavigation = this.CreateMainNavigation(startPageLink);
			layout.SubNavigation = this.CreateSubNavigation(content, startPageLink);

			return layout;
		}

		protected internal virtual IContentRoot<SitePage> CreateMainNavigation(ContentReference startPageLink)
		{
			return this.ContentFacade.Collections.TreeFactory.Create<SitePage>(startPageLink, this.MainNavigationSettings);
		}

		protected internal virtual IContentRoot<SitePage> CreateSubNavigation(IContent content, ContentReference startPageLink)
		{
			ContentReference subNavigationRoot = null;

			// ReSharper disable InvertIf
			if(content != null && !ContentReference.IsNullOrEmpty(startPageLink))
			{
				// We can check some property on the content to get the root-container for the sub-menu.

				var ancestors = this.ContentFacade.Loader.GetAncestors(content.ContentLink).ToArray();

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

			return !ContentReference.IsNullOrEmpty(subNavigationRoot) ? this.ContentFacade.Collections.TreeFactory.Create<SitePage>(subNavigationRoot, this.SubNavigationSettings) : null;
		}

		protected internal virtual void PopulateCultureNavigation(IContent content, IDictionary<CultureInfo, Uri> cultureNavigation)
		{
			if(cultureNavigation == null)
				throw new ArgumentNullException(nameof(cultureNavigation));

			if(!Settings.UIShowGlobalizationUserInterface)
				return;

			if(!(content is ILocalizable localizable))
				return;

			foreach(var culture in localizable.ExistingLanguages)
			{
				cultureNavigation.Add(culture, new Uri(this.ContentFacade.UrlResolver.GetUrl(((IContent) localizable).ContentLink.ToReferenceWithoutVersion(), culture.Name), UriKind.RelativeOrAbsolute));
			}
		}

		#endregion
	}
}