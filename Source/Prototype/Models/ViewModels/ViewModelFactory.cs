using System;
using Prototype.Models.Content;
using Prototype.Models.ViewModels.Shared;

namespace Prototype.Models.ViewModels
{
	public class ViewModelFactory : IViewModelFactory
	{
		#region Constructors

		public ViewModelFactory(IContentContext contentContext, ILayoutFactory layoutFactory)
		{
			this.ContentContext = contentContext ?? throw new ArgumentNullException(nameof(contentContext));
			this.LayoutFactory = layoutFactory ?? throw new ArgumentNullException(nameof(layoutFactory));
		}

		#endregion

		#region Properties

		protected internal virtual IContentContext ContentContext { get; }
		protected internal virtual ILayoutFactory LayoutFactory { get; }

		#endregion

		#region Methods

		public virtual IViewModel Create()
		{
			return new ViewModel(this.ContentContext, this.LayoutFactory);
		}

		#endregion
	}
}