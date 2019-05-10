using MyCompany.MyWebApplication.Models.ViewModels.Shared;

namespace MyCompany.MyWebApplication.Models.ViewModels.Internal
{
	public interface IViewModel
	{
		#region Properties

		ILayout Layout { get; }
		ISettings Settings { get; }

		#endregion

		#region Methods

		void Initialize();

		#endregion
	}
}