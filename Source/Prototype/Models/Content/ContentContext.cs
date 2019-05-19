using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Prototype.Models.Content
{
	/// <summary>
	/// This class needs to be registered scoped (http-context/thread) in the service-container.
	/// </summary>
	public class ContentContext : IContentContext
	{
		#region Fields

		private Lazy<IContentNode> _active;

		#endregion

		#region Constructors

		public ContentContext(IContentRoot contentMap, IHttpContextAccessor httpContextAccessor)
		{
			this.ContentMap = contentMap ?? throw new ArgumentNullException(nameof(contentMap));
			this.HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		#endregion

		#region Properties

		public virtual IContentNode Active
		{
			get
			{
				if(this._active == null)
				{
					// ReSharper disable All
					this._active = new Lazy<IContentNode>(() =>
					{
						var path = this.HttpContextAccessor.HttpContext.Request.Path.Value;

						if(string.IsNullOrEmpty(path))
							return null;

						path = path.TrimEnd('/');

						if(this.IsActive(this.ContentMap, path))
							return this.ContentMap;

						var active = this.ContentMap.Descendants.FirstOrDefault(descendant => this.IsActive(descendant, path));

						if(active == null)
							active = this.ContentMap.Descendants.FirstOrDefault(descendant => descendant.Path.StartsWith(path, StringComparison.OrdinalIgnoreCase));

						return active;
					});
					// ReSharper restore All
				}

				return this._active.Value;
			}
		}

		protected internal virtual IContentRoot ContentMap { get; }
		protected internal virtual IHttpContextAccessor HttpContextAccessor { get; }

		#endregion

		#region Methods

		protected internal virtual bool IsActive(IContentNode contentNode, string path)
		{
			if(contentNode is IContentRoot && this.StringsAreEqual("/" + contentNode.UrlSegment, path))
				return true;

			var url = contentNode?.Url?.ToString().TrimEnd('/');

			return url != null && this.StringsAreEqual(url, path);
		}

		protected internal virtual bool StringsAreEqual(string first, string second)
		{
			if(first == null || second == null)
				return false;

			return string.Equals(first, second, StringComparison.OrdinalIgnoreCase);
		}

		#endregion
	}
}