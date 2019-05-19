using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.Models.Content;

namespace Prototype.Models.Navigation
{
	public class NavigationNode : INavigationNode
	{
		#region Fields

		private bool? _active;
		private bool? _activeAncestor;
		private IEnumerable<IContentNode> _childContents;
		private IEnumerable<NavigationNode> _children;
		private IEnumerable<NavigationNode> _descendants;
		private Lazy<string> _text;

		#endregion

		#region Constructors

		public NavigationNode(IContentNode activeContent, IEnumerable<IContentNode> activeContentAncestors, IContentNode content, int index, INavigationNode parent, INavigationSettings settings)
		{
			this.ActiveContent = activeContent;
			this.ActiveContentAncestors = activeContentAncestors ?? throw new ArgumentNullException(nameof(activeContentAncestors));
			this.Content = content;
			this.Index = index;
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
					this._active = this.ActiveContent != null && this.Content != null && this.ActiveContent == this.Content;

				return this._active.Value;
			}
		}

		public virtual bool ActiveAncestor
		{
			get
			{
				if(this._activeAncestor == null)
					this._activeAncestor = this.ActiveContentAncestors.Any(ancestor => ancestor != null && this.Content != null && ancestor == this.Content);

				return this._activeAncestor.Value;
			}
		}

		protected internal virtual IContentNode ActiveContent { get; }
		protected internal virtual IEnumerable<IContentNode> ActiveContentAncestors { get; }

		protected internal virtual IEnumerable<IContentNode> ChildContents
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._childContents == null)
				{
					var childContents = new List<IContentNode>();

					if(this.Content != null && (this.Settings.Depth == null || this.Settings.Depth.Value > this.Level))
						childContents.AddRange(this.Content.Children);

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
						for(var i = 0; i < this.ChildContents.Count(); i++)
						{
							children.Add(new NavigationNode(this.ActiveContent, this.ActiveContentAncestors, this.ChildContents.ElementAt(i), i, this, this.Settings));
						}
					}

					this._children = children.ToArray();
				}
				// ReSharper restore All

				return this._children;
			}
		}

		public virtual IContentNode Content { get; }

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

		public virtual bool Include => this.Content != null;
		public virtual int Index { get; }
		public virtual bool Leaf => !this.ChildContents.Any();
		public virtual int Level => this.Parent?.Level + 1 ?? 0;
		public virtual INavigationNode Parent { get; }
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

		public virtual Uri Url => this.Content?.Url;

		#endregion

		#region Methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public virtual IEnumerator<INavigationNode> GetEnumerator()
		{
			var self = this.Include ? new[] {this} : Enumerable.Empty<INavigationNode>();

			return self.Concat(this.Descendants).GetEnumerator();
		}

		#endregion
	}
}