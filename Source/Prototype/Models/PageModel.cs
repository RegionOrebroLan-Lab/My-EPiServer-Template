using System;
using Prototype.Models.ViewModels;

namespace Prototype.Models
{
	/// <summary>
	/// Razor page-model, needs to inherit from Microsoft.AspNetCore.Mvc.RazorPages.PageModel.
	/// </summary>
	public class PageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
	{
		#region Fields

		private IViewModel _view;

		#endregion

		#region Constructors

		public PageModel(IViewModelFactory viewModelFactory)
		{
			this.ViewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
		}

		#endregion

		#region Properties

		public virtual IViewModel View => this._view ?? (this._view = this.ViewModelFactory.Create());
		protected internal virtual IViewModelFactory ViewModelFactory { get; }

		#endregion
	}
}