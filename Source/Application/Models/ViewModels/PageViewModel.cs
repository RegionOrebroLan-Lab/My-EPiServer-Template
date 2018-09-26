using System;
using MyCompany.MyWebApplication.Models.Pages;

namespace MyCompany.MyWebApplication.Models.ViewModels
{
	public class PageViewModel<T> : IPageViewModel<T> where T : SitePage
	{
		#region Constructors

		public PageViewModel(T currentPage)
		{
			this.CurrentPage = currentPage ?? throw new ArgumentNullException(nameof(currentPage));
		}

		#endregion

		#region Properties

		public virtual T CurrentPage { get; }

		#endregion
	}
}