using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	public class NavigationNode : INavigationNode
	{
		#region Fields

		private bool? _active;
		private bool? _activeAncestor;
		private IEnumerable<IContent> _childContents;
		private IEnumerable<NavigationNode> _children;
		private IEnumerable<NavigationNode> _descendants;
		private Lazy<string> _text;

		#endregion

		#region Constructors

		public NavigationNode(ContentReference activeLink, IEnumerable<ContentReference> activeLinkAncestors, IContent content, IContentLoader contentLoader, INavigationNode parent, INavigationSettings settings)
		{
			this.ActiveLink = activeLink;
			this.ActiveLinkAncestors = activeLinkAncestors ?? throw new ArgumentNullException(nameof(activeLinkAncestors));
			this.Content = content;
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.Parent = parent;
			this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		#endregion

		#region Properties

		public virtual bool Active
		{
			get
			{
				if(this._active == null)
					this._active = !ContentReference.IsNullOrEmpty(this.ActiveLink) && this.ActiveLink.CompareToIgnoreWorkID(this.Content?.ContentLink);

				return this._active.Value;
			}
		}

		public virtual bool ActiveAncestor
		{
			get
			{
				if(this._activeAncestor == null)
					this._activeAncestor = this.ActiveLinkAncestors.Any(ancestor => !ContentReference.IsNullOrEmpty(ancestor) && ancestor.CompareToIgnoreWorkID(this.Content?.ContentLink));

				return this._activeAncestor.Value;
			}
		}

		protected internal virtual ContentReference ActiveLink { get; }
		protected internal virtual IEnumerable<ContentReference> ActiveLinkAncestors { get; }

		protected internal virtual IEnumerable<IContent> ChildContents
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._childContents == null)
				{
					var childContents = new List<IContent>();

					if(this.Content != null && !ContentReference.IsNullOrEmpty(this.Content.ContentLink) && (this.Settings.Depth == null || this.Settings.Depth.Value > this.Level))
						childContents.AddRange(this.ContentLoader.GetChildren<IContent>(this.Content.ContentLink).Where(content => !this.Settings.Filter.ShouldFilter(content)));

					this._childContents = childContents.ToArray();
				}
				// ReSharper restore InvertIf

				return this._childContents;
			}
		}

		IEnumerable<INavigationNode> INavigationNode.Children => this.Children;

		public virtual IEnumerable<NavigationNode> Children
		{
			get
			{
				// ReSharper disable All
				if(this._children == null)
				{
					var children = new List<NavigationNode>();

					if(this.Active || this.ActiveAncestor || this.Level == 0 || this.Settings.ExpandAll)
					{
						foreach(var child in this.ChildContents)
						{
							children.Add(new NavigationNode(this.ActiveLink, this.ActiveLinkAncestors, child, this.ContentLoader, this, this.Settings));
						}
					}

					this._children = children.ToArray();
				}
				// ReSharper restore All

				return this._children;
			}
		}

		public virtual IContent Content { get; }
		protected internal virtual IContentLoader ContentLoader { get; }

		/// <summary>
		/// Expanded descendants.
		/// </summary>
		protected internal virtual IEnumerable<NavigationNode> Descendants
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._descendants == null)
				{
					var descendants = new List<NavigationNode>();

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

		public virtual bool Leaf => !this.ChildContents.Any();
		public virtual int Level => this.Parent?.Level + 1 ?? 0;
		protected internal virtual INavigationNode Parent { get; }
		protected internal virtual INavigationSettings Settings { get; }

		public virtual string Text
		{
			get
			{
				if(this._text == null)
					this._text = new Lazy<string>(() => this.Settings.TextResolver(this.Content));

				return this._text.Value;
			}
		}

		#endregion
	}
}