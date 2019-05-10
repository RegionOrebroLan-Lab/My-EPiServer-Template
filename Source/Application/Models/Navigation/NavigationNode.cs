using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;

namespace MyCompany.MyWebApplication.Models.Navigation
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class NavigationNode : INavigationNode
	{
		#region Fields

		private IEnumerable<INavigationNode> _children;
		private IEnumerable<IContent> _childrenInternal;
		private static readonly Func<IContent, string> _defaultTextFunction = content => content.Name;
		private IEnumerable<INavigationNode> _descendants;
		private bool? _descendantSelected;
		private bool _exclude;
		private bool? _selected;
		private IEnumerable<ContentReference> _selectedContentAncestors;

		#endregion

		#region Constructors

		public NavigationNode(IContent content, IContentFilter contentFilter, IContentLoader contentLoader, INavigationNode parent, ContentReference selectedContentLink) : this(content, contentFilter, contentLoader, null, false, parent, selectedContentLink) { }
		public NavigationNode(IContent content, IContentFilter contentFilter, IContentLoader contentLoader, int? depth, bool expandAll, INavigationNode parent, ContentReference selectedContentLink) : this(content, contentFilter, contentLoader, depth, expandAll, parent, selectedContentLink, _defaultTextFunction) { }
		public NavigationNode(IContent content, IContentFilter contentFilter, IContentLoader contentLoader, int? depth, bool expandAll, INavigationNode parent, ContentReference selectedContentLink, Func<IContent, string> textFunction) : this(content, contentFilter, contentLoader, depth, expandAll, parent, selectedContentLink, textFunction, true) { }

		protected internal NavigationNode(IContent content, IContentFilter contentFilter, IContentLoader contentLoader, int? depth, bool expandAll, INavigationNode parent, ContentReference selectedContentLink, Func<IContent, string> textFunction, bool validateExclude)
		{
			this.Content = content ?? throw new ArgumentNullException(nameof(content));
			this.ContentFilter = contentFilter ?? throw new ArgumentNullException(nameof(contentFilter));
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.Depth = depth;
			this.ExpandAll = expandAll;
			this.Parent = parent;
			this.SelectedContentLink = selectedContentLink;
			this.TextFunction = textFunction ?? throw new ArgumentNullException(nameof(textFunction));
			this.ValidateExclude = validateExclude;
		}

		#endregion

		#region Properties

		public virtual IEnumerable<INavigationNode> Children
		{
			get
			{
				// ReSharper disable All
				if(this._children == null)
				{
					var children = new List<INavigationNode>();

					if(this.DescendantSelected || this.ExpandAll || this.Level == 0 || this.Selected)
					{
						foreach(var child in this.ChildrenInternal)
						{
							children.Add(new NavigationNode(child, this.ContentFilter, this.ContentLoader, this.Depth, this.ExpandAll, this, this.SelectedContentLink, this.TextFunction, false));
						}
					}

					this._children = children.ToArray();
				}
				// ReSharper restore All

				return this._children;
			}
		}

		protected internal virtual IEnumerable<IContent> ChildrenInternal
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._childrenInternal == null)
				{
					var children = new List<IContent>();

					if(this.Depth == null || this.Depth.Value > this.Level)
						children.AddRange(this.ContentLoader.GetChildren<IContent>(this.Content.ContentLink).Where(content => !this.ContentFilter.ShouldFilter(content)));

					this._childrenInternal = children.ToArray();
				}
				// ReSharper restore InvertIf

				return this._childrenInternal;
			}
		}

		public virtual IContent Content { get; }
		protected internal virtual IContentFilter ContentFilter { get; }
		protected internal virtual IContentLoader ContentLoader { get; }
		protected internal virtual int? Depth { get; }

		public virtual IEnumerable<INavigationNode> Descendants
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._descendants == null)
				{
					var descendants = new List<INavigationNode>();

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

		public virtual bool DescendantSelected
		{
			get
			{
				if(this._descendantSelected == null)
					this._descendantSelected = this.SelectedContentAncestors.Any(contentLink => contentLink.CompareToIgnoreWorkID(this.Content.ContentLink));

				return this._descendantSelected.Value;
			}
		}

		public virtual bool Exclude
		{
			get
			{
				// ReSharper disable InvertIf
				if(!this._exclude && this.ValidateExclude && !this.ExcludeValidated)
				{
					this._exclude = this.ContentFilter.ShouldFilter(this.Content);
					this.ExcludeValidated = true;
				}
				// ReSharper restore InvertIf

				return this._exclude;
			}
			set
			{
				this._exclude = value;
				this.ExcludeValidated = false;
			}
		}

		protected internal virtual bool ExcludeValidated { get; set; }
		protected internal virtual bool ExpandAll { get; }
		public virtual bool Leaf => !this.ChildrenInternal.Any();
		public virtual int Level => this.Parent?.Level + 1 ?? 0;
		public virtual INavigationNode Parent { get; }

		public virtual bool Selected
		{
			get
			{
				if(this._selected == null)
					this._selected = !ContentReference.IsNullOrEmpty(this.SelectedContentLink) && this.SelectedContentLink.CompareToIgnoreWorkID(this.Content.ContentLink);

				return this._selected.Value;
			}
		}

		[SuppressMessage("Style", "IDE0045:Convert to conditional expression")]
		protected internal virtual IEnumerable<ContentReference> SelectedContentAncestors
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._selectedContentAncestors == null)
				{
					if(ContentReference.IsNullOrEmpty(this.SelectedContentLink))
						this._selectedContentAncestors = Enumerable.Empty<ContentReference>();
					else
						this._selectedContentAncestors = this.Parent is NavigationNode parent ? parent.SelectedContentAncestors : this.ContentLoader.GetAncestors(this.SelectedContentLink).Select(content => content.ContentLink);
				}
				// ReSharper restore InvertIf

				return this._selectedContentAncestors;
			}
		}

		protected internal virtual ContentReference SelectedContentLink { get; }
		public virtual string Text => this.TextFunction(this.Content);
		protected internal virtual Func<IContent, string> TextFunction { get; }
		protected internal virtual bool ValidateExclude { get; }

		#endregion

		#region Methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public virtual IEnumerator<INavigationNode> GetEnumerator()
		{
			return new[] {this}.Concat(this.Descendants).Where(node => !node.Exclude).GetEnumerator();
		}

		#endregion
	}
}