using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Prototype.Models.Internal;

namespace Prototype.Models.Content
{
	public class ContentNode : IContentNode
	{
		#region Fields

		private IEnumerable<ContentNode> _ancestors;
		private IEnumerable<ContentNode> _children;
		private IEnumerable<ContentNode> _descendants;
		private Uri _url;

		#endregion

		#region Constructors

		public ContentNode(ContentNode parent, SiteMapNode siteMapNode)
		{
			this.SiteMapNode = siteMapNode ?? throw new ArgumentNullException(nameof(siteMapNode));
			this.Parent = parent;
		}

		#endregion

		#region Properties

		IEnumerable<IContentNode> IContentNode.Ancestors => this.Ancestors;

		public virtual IEnumerable<ContentNode> Ancestors
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._ancestors == null)
				{
					var ancestors = new List<ContentNode>();
					var node = this;

					while(node.Parent != null)
					{
						ancestors.Add(node.Parent);
						node = node.Parent;
					}

					this._ancestors = ancestors.ToArray();
				}
				// ReSharper restore InvertIf

				return this._ancestors;
			}
		}

		IEnumerable<IContentNode> IContentNode.Children => this.Children;

		public virtual IEnumerable<ContentNode> Children
		{
			get
			{
				// ReSharper disable All
				if(this._children == null)
				{
					var children = new List<ContentNode>();

					foreach(var node in this.SiteMapNode.Children)
					{
						children.Add(new ContentNode(this, node));
					}

					this._children = children.ToArray();
				}
				// ReSharper restore All

				return this._children;
			}
		}

		public virtual CultureInfo Culture => this.SiteMapNode.Culture;
		public virtual IEnumerable<CultureInfo> Cultures => this.SiteMapNode.Cultures;
		IEnumerable<IContentNode> IContentNode.Descendants => this.Descendants;

		public virtual IEnumerable<ContentNode> Descendants
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._descendants == null)
				{
					var descendants = new List<ContentNode>();

					foreach(var child in this.Children)
					{
						descendants.Add(child);
						descendants.AddRange(child.Descendants);
					}

					this._descendants = descendants.ToArray();
				}
				// ReSharper restore InvertIf

				return this._descendants;
			}
		}

		public virtual string Description => this.SiteMapNode.Description;
		public virtual string Heading => this.SiteMapNode.Heading;
		public virtual string HtmlContent => this.SiteMapNode.HtmlContent;
		public virtual int Index => this.SiteMapNode.Index;
		public virtual string Introduction => this.SiteMapNode.Introduction;
		public virtual IEnumerable<string> Keywords => this.SiteMapNode.Keywords;
		public virtual string Name => this.SiteMapNode.Name;
		protected internal virtual ContentNode Parent { get; }
		public virtual string Path => this.SiteMapNode.Path;
		protected internal virtual SiteMapNode SiteMapNode { get; }
		public virtual string Title => this.SiteMapNode.Title;

		public virtual Uri Url
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._url == null)
				{
					var segments = this.Ancestors.Reverse().Concat(new[] {this}).Select(node => node.UrlSegmentInternal).Concat(new[] {string.Empty});

					this._url = new Uri(string.Join("/", segments), UriKind.Relative);
				}
				// ReSharper restore InvertIf

				return this._url;
			}
		}

		public virtual string UrlSegment => this.SiteMapNode.UrlSegment;
		protected internal virtual string UrlSegmentInternal => this.UrlSegment;

		#endregion
	}
}