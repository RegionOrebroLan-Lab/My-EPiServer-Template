using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Prototype.Models.Content;
using Prototype.Models.Navigation;

namespace Prototype.Models.ViewModels.Shared
{
	public class LayoutFactory : ILayoutFactory
	{
		#region Fields

		private static INavigationSettings _mainNavigationSettings;
		private static INavigationSettings _subNavigationSettings;

		#endregion

		#region Constructors

		public LayoutFactory(IContentRoot contentMap, INavigationFactory navigationFactory)
		{
			this.ContentMap = contentMap ?? throw new ArgumentNullException(nameof(contentMap));
			this.NavigationFactory = navigationFactory ?? throw new ArgumentNullException(nameof(navigationFactory));
		}

		#endregion

		#region Properties

		protected internal virtual IContentRoot ContentMap { get; }
		public virtual INavigationSettings MainNavigationSettings => _mainNavigationSettings ?? (_mainNavigationSettings = new NavigationSettings {Depth = 1});
		protected internal virtual INavigationFactory NavigationFactory { get; }
		public virtual INavigationSettings SubNavigationSettings => _subNavigationSettings ?? (_subNavigationSettings = new NavigationSettings());

		#endregion

		#region Methods

		public virtual ILayout Create()
		{
			return this.CreateInternal(null);
		}

		public virtual ILayout Create(IContentNode content)
		{
			if(content == null)
				throw new ArgumentNullException(nameof(content));

			return this.CreateInternal(content);
		}

		protected internal virtual ILayout CreateInternal(IContentNode content)
		{
			var layout = new Layout
			{
				MainNavigation = this.CreateMainNavigation(this.ContentMap),
				SubNavigation = this.CreateSubNavigation(content, this.ContentMap)
			};

			this.PopulateCultureNavigation(content, layout.CultureNavigation);

			// ReSharper disable InvertIf
			if(content != null)
			{
				if(content.Culture != null)
					layout.Culture = content.Culture;

				layout.Description = content.Description;

				foreach(var keyword in content.Keywords)
				{
					layout.Keywords.Add(keyword);
				}

				layout.Title = content.Title;
			}
			// ReSharper restore InvertIf

			return layout;
		}

		protected internal virtual INavigationNode CreateMainNavigation(IContentNode root)
		{
			return this.NavigationFactory.Create(root, this.MainNavigationSettings);
		}

		protected internal virtual INavigationNode CreateSubNavigation(IContentNode content, IContentNode root)
		{
			IContentNode subNavigationRoot = null;

			// ReSharper disable InvertIf
			if(content != null && root != null)
			{
				// We can check some property on the content to get the root-container for the sub-menu.

				for(var i = 0; i < content.Ancestors.Count(); i++)
				{
					var ancestor = content.Ancestors.ElementAt(i);

					if(ancestor == root)
					{
						subNavigationRoot = i == 0 ? content : content.Ancestors.ElementAt(i - 1);
						break;
					}
				}
			}
			// ReSharper restore InvertIf

			return this.NavigationFactory.Create(subNavigationRoot, this.SubNavigationSettings);
		}

		protected internal virtual void PopulateCultureNavigation(IContentNode content, IDictionary<CultureInfo, Uri> cultureNavigation)
		{
			if(cultureNavigation == null)
				throw new ArgumentNullException(nameof(cultureNavigation));

			if(content == null)
				return;

			if(content.Culture != null)
				cultureNavigation.Add(content.Culture, content.Url);

			foreach(var culture in content.Cultures.Where(culture => !culture.Equals(content.Culture)))
			{
				var url = new Uri(content.Url + "#" + culture.Name, UriKind.RelativeOrAbsolute);

				cultureNavigation.Add(culture, url);
			}
		}

		#endregion
	}
}