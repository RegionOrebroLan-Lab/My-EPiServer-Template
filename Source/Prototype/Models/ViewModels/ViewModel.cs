using System;
using Prototype.Models.Content;
using Prototype.Models.ViewModels.Shared;

namespace Prototype.Models.ViewModels
{
	public class ViewModel : IViewModel
	{
		#region Fields

		private Lazy<IContentNode> _contentNode;
		private ILayout _layout;

		#endregion

		#region Constructors

		public ViewModel(IContentContext contentContext, ILayoutFactory layoutFactory)
		{
			this.ContentContext = contentContext ?? throw new ArgumentNullException(nameof(contentContext));
			this.LayoutFactory = layoutFactory ?? throw new ArgumentNullException(nameof(layoutFactory));
		}

		#endregion

		#region Properties

		protected internal virtual IContentContext ContentContext { get; }

		protected internal virtual IContentNode ContentNode
		{
			get
			{
				if(this._contentNode == null)
					this._contentNode = new Lazy<IContentNode>(() => this.ContentContext.Active);

				return this._contentNode.Value;
			}
		}

		public virtual string Heading => this.ContentNode?.Heading;
		public virtual string HtmlContent => this.ContentNode?.HtmlContent;
		public virtual string Introduction => this.ContentNode?.Introduction;
		public virtual ILayout Layout => this._layout ?? (this._layout = this.ContentNode != null ? this.LayoutFactory.Create(this.ContentNode) : this.LayoutFactory.Create());
		protected internal virtual ILayoutFactory LayoutFactory { get; }
		public virtual string Name => this.ContentNode?.Name;
		public virtual Uri Url => this.ContentNode?.Url;

		#endregion
	}
}