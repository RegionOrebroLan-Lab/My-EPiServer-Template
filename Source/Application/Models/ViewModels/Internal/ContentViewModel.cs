using System;
using EPiServer.Core;
using MyCompany.MyWebApplication.Models.ViewModels.Shared;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public class ContentViewModel<T> : ViewModel, IContentViewModel<T> where T : IContent
	{
		#region Constructors

		public ContentViewModel(T content, IViewModelFacade facade) : base(facade)
		{
			if(content == null)
				throw new ArgumentNullException(nameof(content));

			this.Content = content;
		}

		#endregion

		#region Properties

		public virtual T Content { get; }

		#endregion

		#region Methods

		protected internal override ILayout CreateLayout()
		{
			return this.Facade.LayoutFactory.Create(this.Content);
		}

		#endregion
	}
}