using System;
using MyCompany.MyWebApplication.Models.ViewModels.Shared;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public abstract class ViewModel : IViewModel
	{
		#region Fields

		private ILayout _layout;

		#endregion

		#region Constructors

		protected ViewModel(IViewModelFacade facade)
		{
			this.Facade = facade ?? throw new ArgumentNullException(nameof(facade));
		}

		#endregion

		#region Properties

		protected internal virtual IViewModelFacade Facade { get; }
		public virtual ILayout Layout => this._layout ?? (this._layout = this.CreateLayout());
		public virtual ISettings Settings => this.Facade.Settings;

		#endregion

		#region Methods

		protected internal virtual ILayout CreateLayout()
		{
			return this.Facade.LayoutFactory.Create();
		}

		public virtual void Initialize() { }

		#endregion
	}
}