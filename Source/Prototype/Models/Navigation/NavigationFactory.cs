using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Prototype.Models.Content;

namespace Prototype.Models.Navigation
{
	public class NavigationFactory : INavigationFactory
	{
		#region Constructors

		public NavigationFactory(IContentContext contentContext, IContentRoot contentMap, IHttpContextAccessor httpContextAccessor)
		{
			this.ContentContext = contentContext ?? throw new ArgumentNullException(nameof(contentContext));
			this.ContentMap = contentMap ?? throw new ArgumentNullException(nameof(contentMap));
		}

		#endregion

		#region Properties

		protected internal virtual IContentContext ContentContext { get; }
		protected internal virtual IContentRoot ContentMap { get; }

		#endregion

		#region Methods

		public virtual INavigationNode Create(IContentNode root, INavigationSettings settings)
		{
			if(settings == null)
				throw new ArgumentNullException(nameof(settings));

			var activeContent = this.ContentContext.Active;
			var activeContentAncestors = (activeContent != null ? activeContent.Ancestors : Enumerable.Empty<IContentNode>()).ToArray();

			return new NavigationRoot(activeContent, activeContentAncestors, root, settings);
		}

		#endregion
	}
}